using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;
    public GameObject settingsPopUp;

    private EntityManager _entityManager;
    private EntityQuery _entityQuery;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entityQuery = _entityManager.CreateEntityQuery(typeof(GameState));
    }

    public void OnClickStart()
    {
        EntityQuery _entityQuery = _entityManager.CreateEntityQuery(typeof(GameState));
        Entity gameStateEntity = _entityQuery.GetSingletonEntity();
        GameState gameState = _entityManager.GetComponentData<GameState>(gameStateEntity);

        gameState.state = 1;
        startPopUp.SetActive(false);

        _entityManager.SetComponentData(gameStateEntity, gameState);
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
