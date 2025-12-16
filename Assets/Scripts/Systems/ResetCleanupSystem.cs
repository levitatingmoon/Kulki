using Unity.Entities;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateAfter(typeof(BrickSpawnerSystem))]
[UpdateAfter(typeof(BallSpawnerSystem))]
[UpdateAfter(typeof(GameOverSystem))]
[UpdateAfter(typeof(BallInputSystem))]
[UpdateAfter(typeof(BallSystem))]
[UpdateAfter(typeof(CollisionSystem))]
public partial class ResetCleanupSystem : SystemBase
{

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (req, entity) in SystemAPI.Query<RefRO<ResetRequest>>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();

    }
}
