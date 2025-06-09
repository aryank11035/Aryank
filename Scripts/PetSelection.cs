using UnityEngine;
using UnityEngine.SceneManagement;

public class PetSelection : MonoBehaviour
{
    public GameObject[] petPrefabs; // array of pet prefabs
    public Transform petContainer;

    private GameObject[] instantiatedPets; // array to hold instantiated pets
    private int currentIndex = 0; //Keeps track of current pet index
    void Start()
    {

        bool petSelected = PlayerPrefs.GetInt("PetSelected", 0) == 1;
        bool petDead = PlayerPrefs.GetInt("PetDead", 0) == 1;

        if (petSelected && !petDead)
        {
            Debug.Log("Pet already selected and alive. Skipping selection screen.");
            SceneManager.LoadScene(1); 
            return;
        }
        // Instantiate all pets at the start and disable them
        instantiatedPets = new GameObject[petPrefabs.Length];

        for(int i = 0; i < petPrefabs.Length; i++)
        {
            instantiatedPets[i] = Instantiate(petPrefabs[i], petContainer);
            instantiatedPets[i].SetActive(false); // only oone prefab will be active
        }
        //show the pet first

        instantiatedPets[currentIndex].SetActive(true);
    }

    public void NextPet()
    {
        Debug.Log("Next Pet is Selected ");

        //to disable the current pet
        instantiatedPets[currentIndex].SetActive(false);

        //increment the index
        currentIndex = (currentIndex + 1) % petPrefabs.Length;

        //Enable the new pet 
        instantiatedPets[currentIndex].SetActive(true);
    }

    public void PreviousPet()
    {
        Debug.Log("Previous Pet is Selected ");

        //to disable the current pet
        instantiatedPets[currentIndex].SetActive(false);

        //decrement the index
        currentIndex = (currentIndex - 1 + petPrefabs.Length) % petPrefabs.Length;

        //Enable the new pet 
        instantiatedPets[currentIndex].SetActive(true);
    }

    public void SelectPet(string petName)
    {
        string selectedPetName = petPrefabs[currentIndex].name;

        //Get the name of the current pet
        PlayerPrefs.SetString("SelectedPet", selectedPetName);
        PlayerPrefs.Save();
        Debug.Log("Selected Pet saved" + selectedPetName);
        SceneManager.LoadScene(1);
    }
}
