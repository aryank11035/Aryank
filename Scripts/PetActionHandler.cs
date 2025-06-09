using UnityEngine;

public class PetActionHandler : MonoBehaviour
{
    public void FeedPet()
    {
        Debug.Log("Trying to get pet...");
        GameObject pet = PetSpawn.GetPet();
        if (pet == null)
        {
            Debug.LogError("No pet found! Make sure PetSpawn is spawning the pet correctly.");
            return;
        }
        Debug.Log("Pet found: " + pet.name);
        if (pet != null)
        {
            pet.GetComponent<MainScript2>()?.Feed();
        }
    }

    public void PlayWithPet()
    {
        GameObject pet = PetSpawn.GetPet();
        if (pet != null)
        {
            pet.GetComponent<MainScript2>()?.Play();
        }
    }

    public void CleanPet()
    {
        GameObject pet = PetSpawn.GetPet();
        if (pet != null)
        {
            pet.GetComponent<MainScript2>()?.Clean();
        }
    }
}
