using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;
using Unity.Physics;

public partial struct BallSpawnerSystem : ISystem
{
    private bool hasShot;
    private float3 shotDirection;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var spawnerQuery = SystemAPI.QueryBuilder().WithAll<SpawnerData>().Build();

        if (spawnerQuery.IsEmpty)
        {
            return;
        }

        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var spawner in SystemAPI.Query<RefRW<SpawnerData>>())
        {
            if (!hasShot && Mouse.current.leftButton.wasPressedThisFrame)
            {
                hasShot = true;
                Camera camera = Camera.main;
                float2 screenPos = Mouse.current.position.ReadValue();
                float3 worldPos = camera.ScreenToWorldPoint(new float3(screenPos, 0));
                worldPos.z = 0;
                Debug.Log("Input: " + worldPos);

                shotDirection = math.normalize(worldPos - float3.zero);

                spawner.ValueRW.ballsToSpawn = spawner.ValueRO.maxBalls;
            }

            spawner.ValueRW.timeLeft -= SystemAPI.Time.DeltaTime;

            if(!hasShot || spawner.ValueRO.ballsToSpawn <= 0 || spawner.ValueRW.timeLeft > 0)
            {
                return;
            }

            spawner.ValueRW.timeLeft = spawner.ValueRO.interval;

            Entity ball = ecb.Instantiate(spawner.ValueRO.prefab);
                
            ecb.SetComponent(ball, new LocalTransform
            {
                Position = spawner.ValueRO.position,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            ecb.SetComponent(ball, new PhysicsVelocity
            {
                Linear = shotDirection * spawner.ValueRO.ballSpeed,
                Angular = float3.zero
            });

            spawner.ValueRW.ballsToSpawn--;
            
        }
     

    }
}
