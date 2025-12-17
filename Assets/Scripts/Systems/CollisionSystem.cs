using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using UnityEngine;

public partial class CollisionSystem : SystemBase
{
    private ComponentLookup<BallData> _ballLookup;
    private ComponentLookup<WallData> _wallLookup;
    private ComponentLookup<BrickData> _brickLookup;

    protected override void OnCreate()
    {
        RequireForUpdate<SimulationSingleton>();
        RequireForUpdate<PhysicsWorldSingleton>();
        _ballLookup = GetComponentLookup<BallData>();
        _wallLookup = GetComponentLookup<WallData>();
        _brickLookup = GetComponentLookup<BrickData>();

        if (!SystemAPI.HasSingleton<ScoreData>())
        {
            var entity = EntityManager.CreateEntity(typeof(ScoreData));
            EntityManager.SetComponentData(entity, new ScoreData { points = 0 });
        }
    }

    protected override void OnUpdate()
    {
        var ballQuery = SystemAPI.QueryBuilder().WithAll<BallData>().Build();
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();

        _ballLookup.Update(this);
        _wallLookup.Update(this);
        _brickLookup.Update(this);

        var scoreHits = new NativeReference<int>(Allocator.TempJob);
        scoreHits.Value = 0;

        var destroyList = new NativeList<Entity>(ballQuery.CalculateEntityCount() * 4, Allocator.TempJob);

        var job = new ColliderJob
        {
            ballLookup = _ballLookup,
            wallLookup = _wallLookup,
            brickLookup = _brickLookup,
            destroyList = destroyList.AsParallelWriter(),
            scoreHits = scoreHits
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

        if (scoreHits.Value > 0)
        {
            var scoreEntity = SystemAPI.GetSingletonEntity<ScoreData>();
            var score = EntityManager.GetComponentData<ScoreData>(scoreEntity);
            score.points += scoreHits.Value;
            EntityManager.SetComponentData(scoreEntity, score);
        }

        scoreHits.Dispose();

    }

    public struct ColliderJob : ICollisionEventsJob
    {
        public ComponentLookup<BallData> ballLookup;
        public ComponentLookup<WallData> wallLookup;
        public ComponentLookup<BrickData> brickLookup;

        public NativeList<Entity>.ParallelWriter destroyList;
        public NativeReference<int> scoreHits;

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

            if(aIsBall && bIsBrick)
            {
                RefRW<BrickData> brick = brickLookup.GetRefRW(collisionEvent.EntityB);
                brick.ValueRW.currentLives -= 1;
                scoreHits.Value += 1;
                if(brick.ValueRO.currentLives <= 0)
                {
                    destroyList.AddNoResize(b);
                }
            }

            if(aIsBrick && bIsBall)
            {
                RefRW<BrickData> brick = brickLookup.GetRefRW(collisionEvent.EntityA);
                brick.ValueRW.currentLives -= 1;
                scoreHits.Value += 1;
                if(brick.ValueRO.currentLives <= 0)
                {
                    destroyList.AddNoResize(a);
                }
            }
        }
    }
}
