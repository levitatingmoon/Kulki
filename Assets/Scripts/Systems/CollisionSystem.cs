using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using UnityEngine;

public partial class CollisionSystem : SystemBase
{
    private ComponentLookup<BallData> ballLookup;
    private ComponentLookup<WallData> wallLookup;
    private ComponentLookup<BrickData> brickLookup;

    protected override void OnCreate()
    {
        RequireForUpdate<SimulationSingleton>();
        RequireForUpdate<PhysicsWorldSingleton>();
        ballLookup = GetComponentLookup<BallData>();
        wallLookup = GetComponentLookup<WallData>();
        brickLookup = GetComponentLookup<BrickData>();
    }

    protected override void OnUpdate()
    {
        var ballQuery = SystemAPI.QueryBuilder().WithAll<BallData>().Build();
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();

        ballLookup.Update(this);
        wallLookup.Update(this);
        brickLookup.Update(this);

        var destroyList = new NativeList<Entity>(ballQuery.CalculateEntityCount(), Allocator.TempJob);

        var job = new ColliderJob
        {
            ballLookup = ballLookup,
            wallLookup = wallLookup,
            brickLookup = brickLookup,
            destroyList = destroyList.AsParallelWriter()
        };

        Dependency = job.Schedule(simulation, Dependency);
        Dependency.Complete();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < destroyList.Length; i++)
        {
            ecb.DestroyEntity(destroyList[i]);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
        destroyList.Dispose();

    }

    public struct ColliderJob : ICollisionEventsJob
    {
        public ComponentLookup<BallData> ballLookup;
        public ComponentLookup<WallData> wallLookup;
        public ComponentLookup<BrickData> brickLookup;

        public NativeList<Entity>.ParallelWriter destroyList;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity a = collisionEvent.EntityA;
            Entity b = collisionEvent.EntityB;
            
            bool aIsBall = ballLookup.HasComponent(a);
            bool bIsBall = ballLookup.HasComponent(b);

            bool aIsWall = wallLookup.HasComponent(a);
            bool bIsWall = wallLookup.HasComponent(b);

            bool aIsBrick = brickLookup.HasComponent(a);
            bool bIsBrick = brickLookup.HasComponent(b);

            if(aIsBall && bIsWall)
            {
                RefRW<WallData> wall = wallLookup.GetRefRW(collisionEvent.EntityB);
                if(wall.ValueRO.isDestroying)
                {
                    //Debug.Log("Destroy ball A");
                    destroyList.AddNoResize(a);
                }
            }

            if(aIsWall && bIsBall)
            {
                RefRW<WallData> wall = wallLookup.GetRefRW(collisionEvent.EntityA);
                if(wall.ValueRO.isDestroying)
                {
                    //Debug.Log("Destroy ball B");
                    destroyList.AddNoResize(b);
                }
            }

            if(aIsBall && bIsBrick)
            {
                RefRW<BrickData> brick = brickLookup.GetRefRW(collisionEvent.EntityB);
                brick.ValueRW.currentLives -= 1;
                if(brick.ValueRO.currentLives <= 0)
                {
                    //Debug.Log("Destroy brick B");
                    destroyList.AddNoResize(b);
                }
            }

            if(aIsBrick && bIsBall)
            {
                RefRW<BrickData> brick = brickLookup.GetRefRW(collisionEvent.EntityA);
                brick.ValueRW.currentLives -= 1;
                if(brick.ValueRO.currentLives <= 0)
                {
                    //Debug.Log("Destroy brick A");
                    destroyList.AddNoResize(a);
                }
            }
        }
    }
}
