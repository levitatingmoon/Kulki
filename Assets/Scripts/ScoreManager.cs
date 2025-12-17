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

        var uiQuery = _entityManager.CreateEntityQuery(typeof(UIState));
        if (!uiQuery.IsEmpty)
        {
            _scoreEntity = uiQuery.GetSingletonEntity();
        }
    }

    void Update()
    {
        if (_scoreEntity != Entity.Null)
        {
            var ui = _entityManager.GetComponentData<UIState>(_scoreEntity);
            scoreText.text = ui.score.ToString();
        }
    }
}
