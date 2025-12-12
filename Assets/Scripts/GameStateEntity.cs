using UnityEngine;
using Unity.Entities;

public class GameStateEntity : MonoBehaviour
{
    void Awake()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = entityManager.CreateEntity();
        entityManager.AddComponentData(entity, new GameState { state = 0 });
    }
}
