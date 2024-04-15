using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    [Header("File saving info")]
    [SerializeField] public string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private SaveDataHandler saveHandler;
    public static DataManager instance { get; private set; }

    private void Awake(){
        if(instance != null){
            Debug.LogError("Multiple data managers in the scene, only use one.");
        }
        instance = this;
        this.saveHandler = new SaveDataHandler(Application.persistentDataPath, fileName + PlayerPrefs.GetInt("profIndex"));
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame(){
        this.gameData = new GameData();
    }
    public void LoadGame(){
        this.gameData = saveHandler.Load();
        if(this.gameData == null){
            NewGame();
        }
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects){
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame(){
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects){
            dataPersistenceObj.SaveData(ref gameData);
        }
        saveHandler.Save(gameData);
    }
    private void Start(){
        //moved to awake to prevent race condition errors depending on which Start was called first -- Caden
        /*
        this.saveHandler = new SaveDataHandler(Application.persistentDataPath, fileName+PlayerPrefs.GetInt("profIndex"));
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
        */
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    private void OnApplicationQuit(){
        SaveGame();
    }
    public void SelectedProfileLoad(int num){
        this.saveHandler = new SaveDataHandler(Application.persistentDataPath, fileName+num);
        //Debug.Log(fileName+num);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
}
