using SQLite4Unity3d;

public class PetStats
{
    [PrimaryKey]
   

    public string SelectedPet { get; set; }

    public float Health { get; set; }
    public float Dirtiness { get; set; }
    public float Happiness { get; set; }
}
