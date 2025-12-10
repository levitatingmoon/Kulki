using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class BallAuthoring : MonoBehaviour
{
    public float speed;

    private class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            
            /*
            AddComponent(entity, new PhysicsVelocity 
            { 
                Linear = float3.zero, 
                Angular = float3.zero 
            });

            AddComponent(entity, new PhysicsDamping 
            { 
                Linear = 0f, 
                Angular = 0f 
            });

            AddComponent(entity, new PhysicsGravityFactor 
            { 
                Value = 0f 
            });
            */
            AddComponent(entity, new BallData 
            { 
                speed = authoring.speed 
            });
        }
    }
}
