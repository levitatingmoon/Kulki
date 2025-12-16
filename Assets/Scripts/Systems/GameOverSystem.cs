using Unity.Entities;
using UnityEngine;

public partial class GameOverSystem : SystemBase
{
    private EntityQuery ballQuery;
    private EntityQuery brickQuery;
    private EntityQuery spawnerQuery;

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
            _wasBallSpawned = false;
        }

        int activeBalls = ballQuery.CalculateEntityCount();
        int remainingBricks = brickQuery.CalculateEntityCount();
        var spawner = SystemAPI.GetSingleton<SpawnerData>();
        bool allShotsFired = spawner.shotsFired >= spawner.shotCount;

        if (activeBalls > 0  && allShotsFired)
        {
            _wasBallSpawned = true;
        }
        
        if (_wasBallSpawned && activeBalls == 0 && gameState.state == 1 && (remainingBricks == 0 || allShotsFired))
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