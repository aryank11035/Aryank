using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  Keeps GameManager across scenes
        }
        else
        {
            Destroy(gameObject); //  Prevents duplicate GameManagers
            return;
        }

        gameObject.SetActive(true); //  Ensures GameManager is always enabled
        Debug.Log("✅ GameManager is active and persistent!");
    }
}
