using UnityEngine;
using Unity.Entities;
using TMPro;

public class UIActivity : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;
    public TMP_Text scoreText;

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

            if (!entityManager.CreateEntityQuery(typeof(ScoreData)).IsEmpty)
            {
                var scoreEntity = entityManager.CreateEntityQuery(typeof(ScoreData)).GetSingletonEntity();
                var score = entityManager.GetComponentData<ScoreData>(scoreEntity).points;
                scoreText.text = "Score: " + score.ToString();
            }

            var entities = gameOverQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
            foreach (var e in entities)
                entityManager.DestroyEntity(e);
            entities.Dispose();
        }
    }
}
