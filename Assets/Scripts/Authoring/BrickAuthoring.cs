using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class BrickAuthoring : MonoBehaviour
{
    public int maxLives;
    public int currentLives;

    private class Baker : Baker<BrickAuthoring>
    {
        public override void Bake (BrickAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickData
            {
                maxLives = authoring.maxLives,
                currentLives = authoring.currentLives
            });
        }
    }
}
