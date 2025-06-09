using UnityEngine;

public class PetSpawn : MonoBehaviour
{
    public GameObject[] petPrefabs;
    public Transform petContainer;

    private static GameObject spawnedPet;

    void Start()
    {
        string selectedPet = PlayerPrefs.GetString("SelectedPet", "DefaultPet");
        Debug.Log(" Selected Pet in GameScene: " + selectedPet);

        if (spawnedPet != null)
        {
            Debug.Log("Pet already spawned. Skipping duplicate spawn.");
            return;
        }
        foreach (GameObject pet in petPrefabs)
        {
            if (pet.name == selectedPet)
            {
                // Instantiate as child of container
                spawnedPet = Instantiate(pet, Vector3.zero, Quaternion.identity, petContainer);

                // ✨ Set localTransform to make sure it spawns exactly inside the container
                spawnedPet.transform.localPosition = Vector3.zero;          // You can customize this
                spawnedPet.transform.localRotation = Quaternion.identity;
                spawnedPet.transform.localScale = Vector3.one;

                spawnedPet.SetActive(true);
                spawnedPet.tag = "Pet"; //  Tag it properly for retrieval

                Debug.Log(" Spawned Pet: " + spawnedPet.name);

                // Ensure MainScript2 is attached
                if (spawnedPet.GetComponent<MainScript2>() == null)
                {
                    spawnedPet.AddComponent<MainScript2>();
                    Debug.Log(" MainScript2 was missing and has been added to pet.");
                }
                else
                {
                    Debug.Log(" MainScript2 already exists on the pet.");
                }

                break;
            }
        }

        if (spawnedPet == null)
        {
            Debug.LogError(" PetSpawn: No pet matched the selected name. Check prefab names.");
        }
    }

    public static GameObject GetPet()
    {
        GameObject pet = GameObject.FindWithTag("Pet");

        if (pet == null)
        {
            Debug.LogWarning(" PetSpawn.GetPet(): No object found with tag 'Pet'.");
        }
        else
        {
            Debug.Log(" Pet found by tag: " + pet.name);
        }

        return pet;
    }
}

