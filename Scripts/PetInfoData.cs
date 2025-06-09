using UnityEngine;

public class PetInfoData : MonoBehaviour
{
    public static PetInfoData Instance;

    public float Health = 100f;
    public float Dirtiness = 0f;
    public float Happiness = 100f;
    public string PetName = "MyPet"; 


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  Persist across scenes
        }
        else
        {
            Destroy(gameObject); //  Prevent duplicate
        }
    }
}
