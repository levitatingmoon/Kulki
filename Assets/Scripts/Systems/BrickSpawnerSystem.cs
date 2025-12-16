using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;
using Unity.Physics;
using Unity.Collections;

partial struct BrickSpawnerSystem : ISystem
{
    static readonly string[] brickLevel = new string[]
    {
        "000000000000000",
        "333333322222233",
        "222221100000111",
        "222222000002222",
        "111100000011111",
        "000000000000000"
    };

    float startX;
    float startY;
    float spacing;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BrickSpawnerData>();

        startX = -7.7f;
        startY = 3.3f;
        spacing = 1.1f;

    }

    public void OnUpdate(ref SystemState state)
    {
        var spawnerEntity = SystemAPI.GetSingletonEntity<BrickSpawnerData>();
        var spawnerData = SystemAPI.GetComponent<BrickSpawnerData>(spawnerEntity);
        var resetQuery = SystemAPI.QueryBuilder().WithAll<ResetRequest>().Build();

        if (!resetQuery.IsEmpty)
        {
            var ecbReset = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (brick, entity) in SystemAPI.Query<RefRW<BrickData>>().WithEntityAccess())
            {
                ecbReset.DestroyEntity(entity);
            }

            spawnerData.spawned = false;
            ecbReset.SetComponent(spawnerEntity, spawnerData);

            ecbReset.Playback(state.EntityManager);
            ecbReset.Dispose();

            return;
        }

        if (spawnerData.spawned)
            return;

        var ecb = SystemAPI
            .GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        //Spawning bricks based on the string
        for (int y = 0; y < brickLevel.Length; y++)
        {
            string row = brickLevel[y];
            for (int x = 0; x < row.Length; x++)
            {
                int lives = row[x] - '0';
                if (lives == 0) continue;

                float3 pos = new float3(startX + x * spacing, startY - y * spacing, -1);

                Entity brickEntity;
                switch (lives)
                {
                    case 2:
                        brickEntity = ecb.Instantiate(spawnerData.prefabTwo);
                        break;
                    case 3:
                        brickEntity = ecb.Instantiate(spawnerData.prefabThree);
                        break;
                    default:
                        brickEntity = ecb.Instantiate(spawnerData.prefabOne);
                        break;
                }

                ecb.SetComponent(brickEntity, LocalTransform.FromPosition(pos));
            }
        }

        spawnerData.spawned = true;
        ecb.SetComponent(spawnerEntity, spawnerData);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}

