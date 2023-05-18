using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    private SavedData savedData;

    [SerializeField] private string fileName;

    private FileDataHandler fileHandler;
    public static DataPersistanceManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        fileHandler = new FileDataHandler(Application.persistentDataPath,fileName);
        Load();
    }
    void NewGame()
    {
        savedData = new SavedData();
    }
    void Save()
    {
        GameManager.instance.playerStats.SaveData(ref savedData);
        fileHandler.SaveData(savedData);
    }

    void Load()
    {
        savedData = fileHandler.LoadData();
        if (savedData == null) NewGame();
        GameManager.instance.SetUpPS();
        GameManager.instance.playerStats.LoadData(savedData);
    }

    public void OnApplicationQuit()
    {
        Save();
    }
}
