using Unity.Burst;
using Unity.Entities;

public partial struct ScoreResetSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.QueryBuilder().WithAll<ResetRequest>().Build().IsEmpty)
            return;

        var scoreEntity = SystemAPI.GetSingletonEntity<ScoreData>();
        state.EntityManager.SetComponentData(scoreEntity, new ScoreData { points = 0 });
    }
}