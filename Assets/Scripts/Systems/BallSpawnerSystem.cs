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
        var ballQuery = SystemAPI.QueryBuilder().WithAll<BallData>().Build();
        int activeBalls = ballQuery.CalculateEntityCount();
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (spawnerQuery.IsEmpty)
        {
            return;
        }

        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        var spawner = SystemAPI.GetSingletonRW<SpawnerData>();

        if (!resetQuery.IsEmpty)
        {
            hasShot = false;
            spawner.ValueRW.shotsFired = 0;
        }

        if (hasShot && activeBalls == 0 && spawner.ValueRO.ballsToSpawn == 0 && spawner.ValueRO.shotsFired < spawner.ValueRO.shotCount)
        {
            hasShot = false;
        }

        if (!hasShot && Mouse.current.leftButton.wasPressedThisFrame && gameState.state == 1)
        {   
            //Set z to 0 because the ball only moves on two axes
            hasShot = true;
            Camera camera = Camera.main;
            float2 screenPos = Mouse.current.position.ReadValue();
            float3 worldPos = camera.ScreenToWorldPoint(new float3(screenPos, 0));
            worldPos.z = 0;
            float3 spawnerPos = spawner.ValueRO.position;
            spawnerPos.z = 0;

            _shotDirection = math.normalize(worldPos - spawnerPos);

            spawner.ValueRW.ballsToSpawn = spawner.ValueRO.maxBalls;
            spawner.ValueRW.shotsFired++;

            spawner.ValueRW.timeLeft = 0f;

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
