using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PetInfoScript : MonoBehaviour
{
    public static PetInfoScript Instance;

    private MainScript2 ms2;
    private GameObject currentPet;

    [SerializeField] private TextMeshProUGUI healtTxt;
    [SerializeField] private TextMeshProUGUI dirtTxt;
    [SerializeField] private TextMeshProUGUI happyTxt;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PetInfoScript is persistent!");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reconnect text references if needed
        if (healtTxt == null || dirtTxt == null || happyTxt == null)
        {
            Debug.Log("Re-assigning TextMeshPro references...");
            healtTxt = GameObject.Find("healthText")?.GetComponent<TextMeshProUGUI>();
            dirtTxt = GameObject.Find("dirtinessText")?.GetComponent<TextMeshProUGUI>();
            happyTxt = GameObject.Find("happinessText")?.GetComponent<TextMeshProUGUI>();
        }

        // Find the pet and set up the info
        Invoke(nameof(FindPetAndSetup), 1.5f); // Delay to ensure scene objects are ready
    }

    void FindPetAndSetup()
    {
        currentPet = PetSpawn.GetPet();

        if (currentPet != null)
        {
            Debug.Log("Pet found: " + currentPet.name);
            ms2 = currentPet.GetComponent<MainScript2>();

            if (ms2 != null)
            {
                Debug.Log("MainScript2 is connected.");
                DisplayInfo();
            }
            else
            {
                Debug.LogError("MainScript2 is missing on pet!");
            }
        }
        else
        {
            Debug.LogWarning("No pet found. Retrying...");
            Invoke(nameof(FindPetAndSetup), 1.5f); // Retry after a delay
        }
    }

    public void DisplayInfo()
    {
        if (ms2 != null && healtTxt != null && dirtTxt != null && happyTxt != null)
        {
            // Get values directly from MainScript2 and update the UI
            healtTxt.text = ms2.getHealth().ToString();
            dirtTxt.text = ms2.getDirt().ToString();
            happyTxt.text = ms2.getHappy().ToString();
        }
       
    }

    // Button Hooks
    public void OnFeedButton()
    {
        if (ms2 != null)
        {
            ms2.Feed();
            DisplayInfo();
        }
    }

    public void OnCleanButton()
    {
        if (ms2 != null)
        {
            ms2.Clean();
            DisplayInfo();
        }
    }

    public void OnPlayButton()
    {
        if (ms2 != null)
        {
            ms2.Play();
            DisplayInfo();
        }
    }
}
