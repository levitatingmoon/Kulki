using Unity.Entities;
using UnityEngine;

public partial class GameOverSystem : SystemBase
{
    private EntityQuery ballQuery;
    private EntityQuery brickQuery;
    private EntityQuery spawnerQuery;

    private bool _wasLastShotBallSpawned = false;
    private bool _wasBallSpawned = false;

    protected override void OnCreate()
    {
        ballQuery = GetEntityQuery(typeof(BallData));

        brickQuery = GetEntityQuery(ComponentType.ReadOnly<BrickData>());
        spawnerQuery = GetEntityQuery(ComponentType.ReadOnly<SpawnerData>());

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

        int activeBalls = ballQuery.CalculateEntityCount();
        int remainingBricks = brickQuery.CalculateEntityCount();
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
        
        if ((_wasLastShotBallSpawned || (_wasBallSpawned && remainingBricks == 0)) && activeBalls == 0 && gameState.state == 1)
        {
            Debug.Log(activeBalls);
            gameState.state = 2;
            SystemAPI.SetSingleton(gameState);
            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponent<GameOverEvent>(entity);
            var query = GetEntityQuery(typeof(GameOverEvent));
        }
        
    }
}


public struct GameOverEvent : IComponentData { }