using UnityEngine.SceneManagement;
using UnityEngine;


public class DeathPopupHandler : MonoBehaviour
{
    public GameObject deathPopup;

    void Start()
    {
        if (deathPopup != null)
            deathPopup.SetActive(false);
    }

    public void ShowDeathPopup()
    {
        if (deathPopup != null)
            deathPopup.SetActive(true);
    }

    public void OnYesButton()
    {
        Debug.Log("Yes clicked - returning to Pet Selection Scene.");
        SceneManager.LoadScene(0); // Or your pet selection scene index
    }

    public void OnNoButton()
    {
        Debug.Log("No clicked - quitting game.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
