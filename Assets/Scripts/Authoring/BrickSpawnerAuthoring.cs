using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class BrickSpawnerAuthoring : MonoBehaviour
{
    public GameObject prefabOne;
    public GameObject prefabTwo;
    public GameObject prefabThree;
    public bool spawned;

    private class Baker : Baker<BrickSpawnerAuthoring>
    {
        public override void Bake(BrickSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            Entity prefabOneEntity = GetEntity(authoring.prefabOne, TransformUsageFlags.Dynamic);
            Entity prefabTwoEntity = GetEntity(authoring.prefabTwo, TransformUsageFlags.Dynamic);
            Entity prefabThreeEntity = GetEntity(authoring.prefabThree, TransformUsageFlags.Dynamic);

            AddComponent(entity, new BrickSpawnerData 
            {
                prefabOne = prefabOneEntity,
                prefabTwo = prefabTwoEntity,
                prefabThree = prefabThreeEntity,
                spawned = authoring.spawned
            });
        }
    }
}

public struct BrickSpawnerData : IComponentData
{
    public Entity prefabOne;
    public Entity prefabTwo;
    public Entity prefabThree;
    public bool spawned;

}