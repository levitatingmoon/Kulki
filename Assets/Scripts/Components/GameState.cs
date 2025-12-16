using Unity.Entities;

public struct GameState : IComponentData
{
    public int state; // 0 = menu, 1 = game, 2 = end
}
