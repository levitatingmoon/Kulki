using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;
    public GameObject settingsPopUp;

    private EntityManager entityManager;
    private EntityQuery entityQuery;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
    }

    public void OnClickStart()
    {
        EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
        Entity gameStateEntity = entityQuery.GetSingletonEntity();
        GameState gameState = entityManager.GetComponentData<GameState>(gameStateEntity);

        gameState.state = 1;
        startPopUp.SetActive(false);

        entityManager.SetComponentData(gameStateEntity, gameState);
    }

    public void OnClickSettings()
    {
        startPopUp.SetActive(false);
        settingsPopUp.SetActive(true);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
    
}
