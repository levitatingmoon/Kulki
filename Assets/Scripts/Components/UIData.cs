using Unity.Entities;
using Unity.Mathematics;

public struct UIState : IComponentData
{
    public int score;
    public bool gameOver;
    public int shotCount;
    public int maxBalls;
    public float3 spawnerPosition;
}
