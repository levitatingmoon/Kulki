using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

public partial struct CollisionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        
    }
}
