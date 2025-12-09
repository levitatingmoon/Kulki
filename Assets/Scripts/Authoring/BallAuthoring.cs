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

            var collider = Unity.Physics.SphereCollider.Create(
                new SphereGeometry { Center = float3.zero, Radius = 0.5f },
                CollisionFilter.Default,
                new Unity.Physics.Material { Friction = 0f, Restitution = 1f });

            var mass = PhysicsMass.CreateDynamic(collider.Value.MassProperties, 1f);
            AddComponent(entity, mass);

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

            AddComponent(entity, new BallData 
            { 
                speed = authoring.speed 
            });
        }
    }
}
