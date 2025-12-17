using Unity.Entities;
using Unity.Mathematics;

public partial class UpdateUIStateSystem : SystemBase
{
    protected override void OnCreate()
    {
        if (!SystemAPI.HasSingleton<UIState>())
        {
            EntityManager.CreateEntity(typeof(UIState));
        }

        RequireForUpdate<UIState>();
    }

    protected override void OnUpdate()
    {
        var ui = SystemAPI.GetSingleton<UIState>();

        if (SystemAPI.HasSingleton<ScoreData>())
        {
            var score = SystemAPI.GetSingleton<ScoreData>();
            ui.score = score.points;
        }

        ui.gameOver = !GetEntityQuery(ComponentType.ReadOnly<GameOverEvent>()).IsEmpty;

        if (SystemAPI.HasSingleton<SpawnerData>())
        {
            var spawner = SystemAPI.GetSingleton<SpawnerData>();
            ui.shotCount = spawner.shotCount;
            ui.maxBalls = spawner.maxBalls;
            ui.spawnerPosition = spawner.position;
        }

        SystemAPI.SetSingleton(ui);
    }
}
