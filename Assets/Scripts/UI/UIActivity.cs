using UnityEngine;
using Unity.Entities;
using TMPro;

public class UIActivity : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;
    public TMP_Text scoreText;

    private EntityManager _entityManager;
    private EntityQuery uiQuery;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        endPopUp.SetActive(false);
    }

    void Update()
    {
        uiQuery = _entityManager.CreateEntityQuery(typeof(UIState));
        
        if (!uiQuery.IsEmpty)
        {
            var ui = uiQuery.GetSingleton<UIState>();
            if (ui.gameOver)
            {
                endPopUp.SetActive(true);
                scoreText.text = "Score: " + ui.score;
            }
        }
    }
}
