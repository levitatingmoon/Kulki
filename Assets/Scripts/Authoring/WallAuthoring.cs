using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public class WallAuthoring : MonoBehaviour
{
    public bool isDestroying;

    private class Baker : Baker<WallAuthoring>
    {
        public override void Bake (WallAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new WallData
            {
                isDestroying = authoring.isDestroying
            });
        }
    }
}
