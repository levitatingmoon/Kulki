using UnityEngine;
using Unity.Entities;
using UnityEngine.InputSystem;

public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer line;
    public float maxDistance = 15f;

    EntityManager em;
    Entity spawnerEntity;
    Entity gameStateEntity;

    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var query = em.CreateEntityQuery(typeof(SpawnerData));
        if (!query.IsEmpty)
        {
            spawnerEntity = query.GetSingletonEntity();
        }

        var gameStateQuery = em.CreateEntityQuery(typeof(GameState));
        if (!gameStateQuery.IsEmpty)
        {
            gameStateEntity = gameStateQuery.GetSingletonEntity();
        }
    }

    void Update()
    {
        if (em.GetComponentData<GameState>(gameStateEntity).state == 1)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 start = em.GetComponentData<SpawnerData>(spawnerEntity).position;
                start.z = 0;
                Vector3 direction = (hit.point - start).normalized;
                Vector3 end = start + direction * maxDistance;

                line.SetPosition(0, start);
                line.SetPosition(1, end);
            }
        }
    }
}
