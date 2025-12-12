using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public bool startGame = false;
    public bool menu = false;
    public GameObject startPopUp;
    public GameObject endPopUp;

    private EntityManager entityManager;
    private EntityQuery entityQuery;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
    }

    public void OnClick()
    {
        EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(GameState));
        Entity gameStateEntity = entityQuery.GetSingletonEntity();
        GameState gameState = entityManager.GetComponentData<GameState>(gameStateEntity);

        if(startGame)
        {
            gameState.state = 1;
            startPopUp.SetActive(false);
        }

        if(menu)
        {
            gameState.state = 0;
            endPopUp.SetActive(false);
            startPopUp.SetActive(true);
            //reset values ex. has shot, spawn bricks again, reset points
        }

        entityManager.SetComponentData(gameStateEntity, gameState);
    }
    
}
