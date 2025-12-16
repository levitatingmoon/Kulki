using UnityEngine;
using Unity.Entities;
using TMPro;

public class BallShotManager : MonoBehaviour
{
    public TMP_Text ballShotText;

    private EntityManager _entityManager;
    private Entity _spawnerEntity;

    private int _balls;
    private int _shots;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var spawnerQuery = _entityManager.CreateEntityQuery(typeof(SpawnerData));
        if (!spawnerQuery.IsEmpty)
        {
            _spawnerEntity = spawnerQuery.GetSingletonEntity();
        }

        _balls = 0;
        _shots = 0;
    }

    void Update()
    {
        int ballsValue = _entityManager.GetComponentData<SpawnerData>(_spawnerEntity).maxBalls;
        int shotValue = _entityManager.GetComponentData<SpawnerData>(_spawnerEntity).shotCount;
        if(_balls != ballsValue && _shots != shotValue)
        {
            _balls = ballsValue;
            _shots = shotValue;
            ballShotText.text = _shots.ToString() + "x" + _balls.ToString();
        }
    }
}
