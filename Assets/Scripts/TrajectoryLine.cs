using UnityEngine;
using Unity.Entities;
using UnityEngine.InputSystem;

public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer line;
    public float maxDistance = 15f;

    private EntityManager _entityManager;
    private Entity _uiEntity;
    private Entity _gameStateEntity;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var uiQuery = _entityManager.CreateEntityQuery(typeof(UIState));
        if (!uiQuery.IsEmpty)
        {
            _uiEntity = uiQuery.GetSingletonEntity();
        }

        var gameStateQuery = _entityManager.CreateEntityQuery(typeof(GameState));
        if (!gameStateQuery.IsEmpty)
        {
            _gameStateEntity = gameStateQuery.GetSingletonEntity();
        }
    }

    void Update()
    {
        //Line to show the direction of the shot, only updated during the game, not in menu
        if (_entityManager.GetComponentData<GameState>(_gameStateEntity).state == 1)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 start = _entityManager.GetComponentData<UIState>(_uiEntity).spawnerPosition;
                start.z = 0;
                Vector3 direction = (hit.point - start).normalized;
                Vector3 end = start + direction * maxDistance;

                line.SetPosition(0, start);
                line.SetPosition(1, end);
            }
        }
    }
}
