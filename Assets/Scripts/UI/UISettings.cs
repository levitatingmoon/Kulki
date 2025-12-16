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

    private EntityManager entityManager;
    private EntityQuery entityQuery;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityQuery = entityManager.CreateEntityQuery(typeof(SpawnerData));

        shotSlider.value = 1;
        ballSlider.value = 15;

        //shotText.text = shotSlider.value.ToString();
        //ballText.text = ballSlider.value.ToString();
    }

    public void OnClickMenu()
    {
        Entity ballSpawnerEntity = entityQuery.GetSingletonEntity();
        SpawnerData ballSpawner = entityManager.GetComponentData<SpawnerData>(ballSpawnerEntity);

        ballSpawner.shotCount = Mathf.RoundToInt(shotSlider.value);
        ballSpawner.maxBalls = Mathf.RoundToInt(ballSlider.value);

        settingsPopUp.SetActive(false);
        startPopUp.SetActive(true);

        entityManager.SetComponentData(ballSpawnerEntity, ballSpawner);
    }

    public void OnClickReset()
    {
        Entity ballSpawnerEntity = entityQuery.GetSingletonEntity();
        SpawnerData ballSpawner = entityManager.GetComponentData<SpawnerData>(ballSpawnerEntity);

        ballSpawner.shotCount = 1;
        ballSpawner.maxBalls = 15;

        entityManager.SetComponentData(ballSpawnerEntity, ballSpawner);

        shotSlider.value = 1;
        ballSlider.value = 15;
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
