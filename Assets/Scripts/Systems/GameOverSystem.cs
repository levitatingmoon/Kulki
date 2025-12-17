using Unity.Entities;
using UnityEngine;

public partial class GameOverSystem : SystemBase
{
    private EntityQuery _ballQuery;
    private EntityQuery _brickQuery;
    private EntityQuery _spawnerQuery;

    private bool _wasLastShotBallSpawned = false;
    private bool _wasBallSpawned = false;

    protected override void OnCreate()
    {
        _ballQuery = GetEntityQuery(typeof(BallData));

        _brickQuery = GetEntityQuery(ComponentType.ReadOnly<BrickData>());
        _spawnerQuery = GetEntityQuery(ComponentType.ReadOnly<SpawnerData>());

        RequireForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var resetQuery = SystemAPI.QueryBuilder().WithAll<ResetRequest>().Build();
        var gameState = SystemAPI.GetSingleton<GameState>();

        if (!resetQuery.IsEmpty)
        {
            _wasLastShotBallSpawned = false;
            _wasBallSpawned = false;
        }

        int activeBalls = _ballQuery.CalculateEntityCount();
        int remainingBricks = _brickQuery.CalculateEntityCount();
        var spawner = SystemAPI.GetSingleton<SpawnerData>();
        bool allShotsFired = spawner.shotsFired >= spawner.shotCount;

        if (activeBalls > 0)
        {
            _wasBallSpawned = true;

            if (allShotsFired)
            {
                _wasLastShotBallSpawned = true;
            }
        }
        
        //Game over if all shots were fired and all balls disappeared or if all bricks are gone
        if ((_wasLastShotBallSpawned || (_wasBallSpawned && remainingBricks == 0)) && activeBalls == 0 && gameState.state == 1)
        {
            gameState.state = 2;
            SystemAPI.SetSingleton(gameState);

            if (SystemAPI.TryGetSingleton<UIState>(out var ui))
            {
                ui.gameOver = true;
                SystemAPI.SetSingleton(ui);
            }
        }
        
    }
}


public struct GameOverEvent : IComponentData { }