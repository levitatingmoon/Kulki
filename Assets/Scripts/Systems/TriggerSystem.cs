using Unity.Entities;
using Unity.Physics;
using Unity.Collections;

public partial struct TriggerSystem : ISystem
{
    private ComponentLookup<BallData> ballLookup;
    private ComponentLookup<WallData> wallLookup;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();

        ballLookup = state.GetComponentLookup<BallData>(true);
        wallLookup = state.GetComponentLookup<WallData>(true);
    }

    public void OnUpdate(ref SystemState state)
    {
        ballLookup.Update(ref state);
        wallLookup.Update(ref state);

        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        var job = new TriggerJob
        {
            ballLookup = ballLookup,
            wallLookup = wallLookup,
            ecb = ecb.AsParallelWriter()
        };

        state.Dependency = job.Schedule(
            SystemAPI.GetSingleton<SimulationSingleton>(),
            state.Dependency
        );

        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private struct TriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<BallData> ballLookup;
        [ReadOnly] public ComponentLookup<WallData> wallLookup;

        public EntityCommandBuffer.ParallelWriter ecb;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity a = triggerEvent.EntityA;
            Entity b = triggerEvent.EntityB;

            bool aIsBall = ballLookup.HasComponent(a);
            bool bIsBall = ballLookup.HasComponent(b);

            bool aIsWall = wallLookup.HasComponent(a);
            bool bIsWall = wallLookup.HasComponent(b);

            if (aIsWall && bIsBall)
            {
                ecb.DestroyEntity(0, b);
            }
            else if (bIsWall && aIsBall)
            {
                ecb.DestroyEntity(0, a);
            }
        }
    }
}
