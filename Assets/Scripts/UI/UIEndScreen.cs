using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;

    private EntityManager _entityManager;
    private EntityQuery _entityQuery;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entityQuery = _entityManager.CreateEntityQuery(typeof(GameState));
    }

    public void OnClickMenu()
    {
        EntityQuery _entityQuery = _entityManager.CreateEntityQuery(typeof(GameState));
        Entity gameStateEntity = _entityQuery.GetSingletonEntity();
        GameState gameState = _entityManager.GetComponentData<GameState>(gameStateEntity);

        gameState.state = 0;
        endPopUp.SetActive(false);
        startPopUp.SetActive(true);
        _entityManager.CreateEntity(typeof(ResetRequest));

        _entityManager.SetComponentData(gameStateEntity, gameState);
    }
    
}
