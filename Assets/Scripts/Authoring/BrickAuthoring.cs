using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class BrickAuthoring : MonoBehaviour
{
    public int lives;

    private class Baker : Baker<BrickAuthoring>
    {
        public override void Bake (BrickAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BrickData
            {
                lives = authoring.lives
            });
        }
    }
}
