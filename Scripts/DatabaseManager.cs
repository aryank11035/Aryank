using UnityEngine;
using System.IO;
using SQLite4Unity3d;
using UnityEngine.InputSystem.LowLevel;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;

    private SQLiteConnection _connection;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDatabase()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "TamagotchiDB.db");
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _connection.CreateTable<PetStats>();
        Debug.Log(" Database initialized at: " + dbPath);
    }

    public void SavePetData(PetStats data)
    {
        _connection.InsertOrReplace(data);
        Debug.Log("Pet data saved: Health = " + data.Health + ", Dirtiness = " + data.Dirtiness + ", Happiness = " + data.Happiness);
    }

    public PetStats LoadPetData(string petName)
    {
        return _connection.Find<PetStats>(petName);
    }


}
