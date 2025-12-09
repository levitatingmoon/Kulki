using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class BallSpawnerAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public float interval;

    private class Baker : Baker<BallSpawnerAuthoring>
    {
        public override void Bake(BallSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnerData 
            {
                prefab = GetEntity(authoring.prefab),
                interval = authoring.interval,
                timeLeft = authoring.interval,
                position = authoring.transform.position
            });
        }
    }
}

public struct SpawnerData : IComponentData
{
    public Entity prefab;
    public float interval;
    public float timeLeft;
    public float3 position;
}
