using Unity.Entities;
using UnityEngine;

public partial class BallCountSystem : SystemBase
{
    private EntityQuery ballQuery;
    private bool _wasBallSpawned = false;

    protected override void OnCreate()
    {
        ballQuery = GetEntityQuery(typeof(BallData));

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

        int count = ballQuery.CalculateEntityCount();
        if (count > 0)
        {
            _wasBallSpawned = true;
        }
        
        if (count == 0 && gameState.state == 1 && _wasBallSpawned)
        {
            gameState.state = 2;
            SystemAPI.SetSingleton(gameState);
            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponent<GameOverEvent>(entity);
            var query = GetEntityQuery(typeof(GameOverEvent));
        }
        
    }
}


public struct GameOverEvent : IComponentData { }