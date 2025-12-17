using UnityEngine;
using Unity.Entities;
using TMPro;

public class BallShotManager : MonoBehaviour
{
    public TMP_Text ballShotText;

    private EntityManager _entityManager;
    private Entity _uiEntity;

    private int _balls;
    private int _shots;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var uiQuery = _entityManager.CreateEntityQuery(typeof(UIState));
        if (!uiQuery.IsEmpty)
        {
            _uiEntity = uiQuery.GetSingletonEntity();
        }

        _balls = 0;
        _shots = 0;
    }

    void Update()
    {
        int ballsValue = _entityManager.GetComponentData<UIState>(_uiEntity).maxBalls;
        int shotValue = _entityManager.GetComponentData<UIState>(_uiEntity).shotCount;
        if(_balls != ballsValue || _shots != shotValue)
        {
            _balls = ballsValue;
            _shots = shotValue;
            ballShotText.text = _shots.ToString() + "x" + _balls.ToString();
        }
    }
}
