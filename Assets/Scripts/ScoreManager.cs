using UnityEngine;
using Unity.Entities;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;

    EntityManager em;
    Entity scoreEntity;

    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;

        var query = em.CreateEntityQuery(typeof(ScoreData));
        if (query.IsEmpty)
        {
            scoreEntity = em.CreateEntity(typeof(ScoreData));
            em.SetComponentData(scoreEntity, new ScoreData { points = 0 });
        }
        else
        {
            scoreEntity = query.GetSingletonEntity();
        }
    }

    void Update()
    {
        int score = em.GetComponentData<ScoreData>(scoreEntity).points;
        scoreText.text = score.ToString();
    }
}
