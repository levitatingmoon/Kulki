using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Unity.Physics;

public partial struct BallInputSystem : ISystem
{
    private bool hasShot;

    public void OnUpdate(ref SystemState state)
    {
        
        if(hasShot)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            hasShot = true;
            Camera camera = Camera.main;
            float2 screenPos = Mouse.current.position.ReadValue();
            float3 worldPos = camera.ScreenToWorldPoint(new float3(screenPos, 0));
            worldPos.z = 0;
            Debug.Log("Input: " + worldPos);

            float3 direction = math.normalize(worldPos - float3.zero);

            foreach (var (vel, ball) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRO<BallData>>())
            {
                vel.ValueRW.Linear = direction * ball.ValueRO.speed;
            }
        }
    
    }
}
