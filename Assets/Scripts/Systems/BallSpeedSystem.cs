using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;

public partial struct BallSpeedSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (velocity, ball) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<BallData>>().WithAll<BallData>())
        {
            float3 v = velocity.ValueRW.Linear;
            if (math.lengthsq(v) > 0f)
            {
                velocity.ValueRW.Linear = math.normalize(v) * ball.ValueRO.speed;
            }
        }
    }
}
