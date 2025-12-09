using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct BallSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BallData>();
    }

    public void OnUpdate(ref SystemState state)
    {
    }

}
