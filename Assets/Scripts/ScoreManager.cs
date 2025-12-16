using UnityEngine;
using Unity.Entities;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;

    private EntityManager _entityManager;
    private Entity _scoreEntity;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var query = _entityManager.CreateEntityQuery(typeof(ScoreData));
        if (query.IsEmpty)
        {
            _scoreEntity = _entityManager.CreateEntity(typeof(ScoreData));
            _entityManager.SetComponentData(_scoreEntity, new ScoreData { points = 0 });
        }
        else
        {
            _scoreEntity = query.GetSingletonEntity();
        }
    }

    void Update()
    {
        int score = _entityManager.GetComponentData<ScoreData>(_scoreEntity).points;
        scoreText.text = score.ToString();
    }
}
