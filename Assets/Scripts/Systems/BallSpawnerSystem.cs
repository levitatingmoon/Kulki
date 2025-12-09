using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct BallSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach(var spawner in SystemAPI.Query<RefRW<SpawnerData>>())
        {
            spawner.ValueRW.timeLeft -= SystemAPI.Time.DeltaTime;
            if(spawner.ValueRO.timeLeft < 0)
            {
                spawner.ValueRW.timeLeft = spawner.ValueRO.interval;
                Entity ball = ecb.Instantiate(spawner.ValueRO.prefab);
                
                ecb.SetComponent(ball, new LocalTransform
                {
                    Position = spawner.ValueRO.position,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });
                
            }
        }
    }
}
