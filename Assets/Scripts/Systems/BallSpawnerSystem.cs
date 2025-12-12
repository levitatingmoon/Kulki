using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;
using Unity.Physics;

public partial struct BallSpawnerSystem : ISystem
{
    public bool hasShot;
    private float3 _shotDirection;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var spawnerQuery = SystemAPI.QueryBuilder().WithAll<SpawnerData>().Build();
        var resetQuery = SystemAPI.QueryBuilder().WithAll<ResetRequest>().Build();
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (spawnerQuery.IsEmpty)
        {
            return;
        }

        if (!resetQuery.IsEmpty)
        {
            hasShot = false;
        }

        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var spawner in SystemAPI.Query<RefRW<SpawnerData>>())
        {
            if (!hasShot && Mouse.current.leftButton.wasPressedThisFrame && gameState.state == 1)
            {   
                hasShot = true;
                Camera camera = Camera.main;
                float2 screenPos = Mouse.current.position.ReadValue();
                float3 worldPos = camera.ScreenToWorldPoint(new float3(screenPos, 0));
                worldPos.z = 0;
                float3 spawnerPos = spawner.ValueRO.position;
                spawnerPos.z = 0;
                Debug.Log("Input: " + worldPos);

                _shotDirection = math.normalize(worldPos - spawnerPos);

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
                Scale = 0.5f
            });

            ecb.SetComponent(ball, new PhysicsVelocity
            {
                Linear = _shotDirection * spawner.ValueRO.ballSpeed,
                Angular = float3.zero
            });

            spawner.ValueRW.ballsToSpawn--;
            
        }
     

    }
}
