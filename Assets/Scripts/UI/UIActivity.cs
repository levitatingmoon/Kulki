using UnityEngine;
using Unity.Entities;
using TMPro;

public class UIActivity : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject endPopUp;
    public TMP_Text scoreText;

    private EntityManager _entityManager;
    private EntityQuery _entityQuery;
    private EntityQuery _gameOverQuery;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entityQuery = _entityManager.CreateEntityQuery(typeof(GameState));
        _gameOverQuery = _entityManager.CreateEntityQuery(typeof(GameOverEvent));
        endPopUp.SetActive(false);
    }

    void Update()
    {
        if (_gameOverQuery.CalculateEntityCount() > 0)
        {
            endPopUp.SetActive(true);

            if (!_entityManager.CreateEntityQuery(typeof(ScoreData)).IsEmpty)
            {
                var scoreEntity = _entityManager.CreateEntityQuery(typeof(ScoreData)).GetSingletonEntity();
                var score = _entityManager.GetComponentData<ScoreData>(scoreEntity).points;
                scoreText.text = "Score: " + score.ToString();
            }

            var entities = _gameOverQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
            foreach (var e in entities)
            {
                _entityManager.DestroyEntity(e);
            }
            entities.Dispose();
        }
    }
}
