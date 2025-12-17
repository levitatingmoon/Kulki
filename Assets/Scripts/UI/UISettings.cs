using UnityEngine;
using Unity.Entities;
using TMPro;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public GameObject startPopUp;
    public GameObject settingsPopUp;

    public Slider shotSlider;
    public Slider ballSlider;

    public TMP_Text shotText;
    public TMP_Text ballText;

    private EntityManager _entityManager;
    private EntityQuery _entityQuery;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _entityQuery = _entityManager.CreateEntityQuery(typeof(SpawnerData));

        shotSlider.value = 1;
        ballSlider.value = 15;
    }

    public void OnClickMenu()
    {
        Entity ballSpawnerEntity = _entityQuery.GetSingletonEntity();
        SpawnerData ballSpawner = _entityManager.GetComponentData<SpawnerData>(ballSpawnerEntity);

        ballSpawner.shotCount = Mathf.RoundToInt(shotSlider.value);
        ballSpawner.maxBalls = Mathf.RoundToInt(ballSlider.value);

        settingsPopUp.SetActive(false);
        startPopUp.SetActive(true);

        _entityManager.SetComponentData(ballSpawnerEntity, ballSpawner);

        var uiEntity = _entityManager.CreateEntityQuery(typeof(UIState)).GetSingletonEntity();
        var ui = _entityManager.GetComponentData<UIState>(uiEntity);
        ui.shotCount = ballSpawner.shotCount;
        ui.maxBalls = ballSpawner.maxBalls;
        _entityManager.SetComponentData(uiEntity, ui);
    }

    public void OnClickReset()
    {
        Entity ballSpawnerEntity = _entityQuery.GetSingletonEntity();
        SpawnerData ballSpawner = _entityManager.GetComponentData<SpawnerData>(ballSpawnerEntity);

        ballSpawner.shotCount = 1;
        ballSpawner.maxBalls = 15;

        _entityManager.SetComponentData(ballSpawnerEntity, ballSpawner);

        shotSlider.value = 1;
        ballSlider.value = 15;

        if (_entityManager.HasComponent<UIState>(ballSpawnerEntity))
        {
            var ui = _entityManager.GetComponentData<UIState>(ballSpawnerEntity);
            ui.shotCount = ballSpawner.shotCount;
            ui.maxBalls = ballSpawner.maxBalls;
            _entityManager.SetComponentData(ballSpawnerEntity, ui);
        }
    }

    public void UpdateShotValue()
    {
        shotText.text = shotSlider.value.ToString();
    }

    public void UpdateBallValue()
    {
        ballText.text = ballSlider.value.ToString();
    }
    
}
