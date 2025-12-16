using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;

    private EntityManager entityManager;
    private EntityQuery entityQuery;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
    }

    public void OnClickMenu()
    {
        EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
        Entity gameStateEntity = entityQuery.GetSingletonEntity();
        GameState gameState = entityManager.GetComponentData<GameState>(gameStateEntity);

        gameState.state = 0;
        endPopUp.SetActive(false);
        startPopUp.SetActive(true);
        entityManager.CreateEntity(typeof(ResetRequest));

        entityManager.SetComponentData(gameStateEntity, gameState);
    }
    
}
