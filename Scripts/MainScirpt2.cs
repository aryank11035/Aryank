using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainScript2 : MonoBehaviour
{
    PetInfoScript petInfo;

    [SerializeField] private float health_value = 100f;
    [SerializeField] private float happy_value = 100f;
    [SerializeField] private float dirt_value = 0f;

    [SerializeField] private float maxStatValue = 100f;

    [SerializeField] private float dirtIncreaseRate = 5f / (12f * 60f);
    [SerializeField] private float happinessDecreaseRate = 1f;
    [SerializeField] private float healthDecreaseRate = 1f;

    [SerializeField] private float feedAmount = 10f;
    [SerializeField] private float happinessIncreaseAmount = 15f;

    [SerializeField] private float petLifespan = 3f * 60f * 60f;

    private DateTime startTime;
    private bool isPetDead = false;

    private float lastDirtThreshold = 0f;
    private float lastHappyThreshold = 100f;

    void Start()
    {
        petInfo = FindAnyObjectByType<PetInfoScript>();

        LoadSavedData();

        startTime = DateTime.Now;

        if (petInfo == null)
        {
            Debug.LogError("Pet Info script not found!");
        }
    }

    void Update()
    {
        if (isPetDead) return;

        // Increment dirt value and clamp between 0 and max
        dirt_value += dirtIncreaseRate * Time.deltaTime;
        dirt_value = Mathf.Clamp(dirt_value, 0, maxStatValue);

        // Check if dirt threshold is exceeded to decrease happiness
        if (dirt_value >= lastDirtThreshold + 2.5f)
        {
            float decreaseRate = (dirt_value >= maxStatValue) ? happinessDecreaseRate * 2f : happinessDecreaseRate;

            happy_value -= decreaseRate;
            happy_value = Mathf.Clamp(happy_value, 0, maxStatValue);
            lastDirtThreshold += 2.5f;
        }

        // Check happiness value to decrease health
        if (lastHappyThreshold - happy_value >= 2f)
        {
            float decreaseRate = healthDecreaseRate;

            if (happy_value <= 0)
                decreaseRate *= 3f;
            else if (dirt_value >= maxStatValue)
                decreaseRate *= 2f;

            health_value -= decreaseRate;
            health_value = Mathf.Clamp(health_value, 0, maxStatValue);
            lastHappyThreshold = happy_value;
        }

        // If dirt value exceeds max, further decrease happiness and health
        if (dirt_value >= maxStatValue)
        {
            happy_value -= happinessDecreaseRate * 2f * Time.deltaTime;
            happy_value = Mathf.Clamp(happy_value, 0, maxStatValue);

            health_value -= (happy_value <= 0 ? healthDecreaseRate * 3f : healthDecreaseRate * 2f) * Time.deltaTime;
            health_value = Mathf.Clamp(health_value, 0, maxStatValue);
        }

        // Check if pet lifespan is over or health is 0 (pet died)
        if ((DateTime.Now - startTime).TotalSeconds >= petLifespan || health_value <= 0)
        {
            PetDied("Pet died after 3 hours or due to low health!");
            return;
        }

        // Update UI with the latest values if petInfo is not null
        if (petInfo != null)
        {
            petInfo.DisplayInfo();
        }

        // Update singleton stats every frame
        UpdatePetInfoData();
    }

    void PetDied(string reason)
    {
        Debug.Log("Pet died! Reason: " + reason);
        isPetDead = true;
    }

    void LoadSavedData()
    {
        string selectedPet = PlayerPrefs.GetString("SelectedPet", "dog1");
        PetStats loadedStats = DatabaseManager.Instance.LoadPetData(selectedPet);

        if (loadedStats != null)
        {
            // Update the MainScript2 values directly
            health_value = loadedStats.Health;
            dirt_value = loadedStats.Dirtiness;
            happy_value = loadedStats.Happiness;

            Debug.Log("Loaded data for " + selectedPet + ": Health = " + loadedStats.Health + ", Dirtiness = " + loadedStats.Dirtiness + ", Happiness = " + loadedStats.Happiness);

            // Update PetInfoData if needed
            PetInfoData.Instance.Health = loadedStats.Health;
            PetInfoData.Instance.Dirtiness = loadedStats.Dirtiness;
            PetInfoData.Instance.Happiness = loadedStats.Happiness;

            Debug.Log("Data loaded from database!");

            // Update the UI immediately
            if (petInfo != null)
            {
                petInfo.DisplayInfo();
            }
        }
        else
        {
            Debug.LogWarning("No saved data found for pet: " + selectedPet);
        }
    }

    public void ResetPetData()
    {
        health_value = 100f;
        happy_value = 100f;
        dirt_value = 0f;
        lastDirtThreshold = 0f;
        lastHappyThreshold = 100f;
        startTime = DateTime.Now;
        isPetDead = false;
        Debug.Log("Pet reset.");
    }

    public void Feed()
    {
        if (isPetDead) return;

        health_value += feedAmount;
        health_value = Mathf.Clamp(health_value, 0, maxStatValue);
        Debug.Log("Pet fed");
        UpdatePetInfoData();
    }

    public void Clean()
    {
        if (isPetDead) return;

        dirt_value = 0;
        lastDirtThreshold = 0f;
        Debug.Log("Pet cleaned.");
        UpdatePetInfoData();
    }

    public void Play()
    {
        if (isPetDead) return;

        happy_value += happinessIncreaseAmount;
        happy_value = Mathf.Clamp(happy_value, 0, maxStatValue);
        Debug.Log("Pet played");
        UpdatePetInfoData();
    }

    public float getHealth() => Mathf.RoundToInt(health_value);
    public float getDirt() => Mathf.RoundToInt(dirt_value);
    public float getHappy() => Mathf.RoundToInt(happy_value);

    void UpdatePetInfoData()
    {
        if (PetInfoData.Instance != null)
        {
            PetInfoData.Instance.Health = health_value;
            PetInfoData.Instance.Dirtiness = dirt_value;
            PetInfoData.Instance.Happiness = happy_value;
        }
    }

    void OnDestroy()
    {
        Debug.LogError($" {gameObject.name} (MainScript2) is being DESTROYED!");
    }
}
