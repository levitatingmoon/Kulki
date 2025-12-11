using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Unity.Physics;
using Unity.Transforms;

public partial struct BallInputSystem : ISystem
{
    private bool _hasShot;

    public void OnUpdate(ref SystemState state)
    {
        
        if(_hasShot)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _hasShot = true;
            Camera camera = Camera.main;
            float2 screenPos = Mouse.current.position.ReadValue();
            float3 worldPos = camera.ScreenToWorldPoint(new float3(screenPos, 0));
            worldPos.z = 0;
            Debug.Log("Input: " + worldPos);


            foreach (var (vel, ball, transform) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<BallData>, RefRO<LocalTransform>>())
            {
                float3 ballPos = transform.ValueRO.Position;
                ballPos.z = 0;
                float3 direction = math.normalize(worldPos - ballPos);
                vel.ValueRW.Linear = direction * ball.ValueRO.speed;
            }
        }
    
    }
}
