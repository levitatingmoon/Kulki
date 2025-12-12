using UnityEngine;
using Unity.Entities;

public class UIActivity : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;

    private EntityManager entityManager;
    private EntityQuery entityQuery;
    private EntityQuery gameOverQuery;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
        gameOverQuery = entityManager.CreateEntityQuery(typeof(GameOverEvent));
        endPopUp.SetActive(false);
    }

    void Update()
    {
        if (gameOverQuery.CalculateEntityCount() > 0)
        {
            endPopUp.SetActive(true);

            var entities = gameOverQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
            foreach (var e in entities)
                entityManager.DestroyEntity(e);
            entities.Dispose();
        }
    }
}
