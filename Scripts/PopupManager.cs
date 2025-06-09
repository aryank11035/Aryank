
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class PopupManager : MonoBehaviour
{
    public GameObject confirmationPopUp;

    private enum PopupType { None, Death, Selection }
    private PopupType currentPopupType = PopupType.None;

    void Start()
    {
        if (confirmationPopUp != null)
            confirmationPopUp.SetActive(false);
    }

    public void ShoPopup()
    {
         confirmationPopUp.SetActive(true);
    }
    
    public void ShowDeathPopup()
    {
        currentPopupType = PopupType.Death;

        if (confirmationPopUp != null)
            confirmationPopUp.SetActive(true);
    }

    // Call this from SELECT button in Pet Selection scene
    public void ShowSelectionPopup()
    {
        currentPopupType = PopupType.Selection;

        if (confirmationPopUp != null)
            confirmationPopUp.SetActive(true);
    }

    // YES button → perform based on popup type
    public void OnYesButton()
    {
        if (currentPopupType == PopupType.Death)
        {
            Debug.Log("Yes clicked - returning to Pet Selection Scene.");
            SceneManager.LoadScene(0); // Pet Selection
        }
        else if (currentPopupType == PopupType.Selection)
        {
            Debug.Log("Yes clicked - loading main pet scene.");
            SceneManager.LoadScene(1); // Or whichever is your main scene
        }
    }

    // NO button → cancel popup or quit
    public void OnNoButton()
    {
        if (currentPopupType == PopupType.Death)
        {
            Debug.Log("No clicked - saving & quitting.");
            SaveToDatabase();
            CloseGame();
        }
        else if (currentPopupType == PopupType.Selection)
        {
            Debug.Log("Selection canceled.");
            confirmationPopUp.SetActive(false);
        }
    }

    public void CloseGame()
    {
        if (PetInfoData.Instance != null)
        {
            PlayerPrefs.SetInt("Health", Mathf.RoundToInt(PetInfoData.Instance.Health));
            PlayerPrefs.SetInt("Dirtiness", Mathf.RoundToInt(PetInfoData.Instance.Dirtiness));
            PlayerPrefs.SetInt("Happiness", Mathf.RoundToInt(PetInfoData.Instance.Happiness));
            PlayerPrefs.Save();
            Debug.Log("Game data saved before quitting.");

            SaveToDatabase();
        }

        Application.Quit();
    }

    public void PrintSavedPrefs()
    {
        Debug.Log("SAVED PlayerPrefs DATA:");
        Debug.Log("SelectedPet: " + PlayerPrefs.GetString("SelectedPet", "Not Found"));
        Debug.Log("PetName: " + PlayerPrefs.GetString("PetName", "Not Found"));
        Debug.Log("Health: " + PlayerPrefs.GetFloat("Health", -1));
        Debug.Log("Dirtiness: " + PlayerPrefs.GetFloat("Dirtiness", -1));
        Debug.Log("Happiness: " + PlayerPrefs.GetFloat("Happiness", -1));
    }

    public void SaveToDatabase()
    {
        if (PetInfoData.Instance == null) return;

        var stats = new PetStats
        {
            SelectedPet = PlayerPrefs.GetString("SelectedPet", "dog1"),
            Health = PetInfoData.Instance.Health,
            Dirtiness = PetInfoData.Instance.Dirtiness,
            Happiness = PetInfoData.Instance.Happiness
        };

        DatabaseManager.Instance.SavePetData(stats);
    }
}
