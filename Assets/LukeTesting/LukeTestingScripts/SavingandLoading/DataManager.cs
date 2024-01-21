using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataManager instance { get; private set; }

    private void Awake(){
        if(instance != null){
            Debug.LogError("Multiple data managers in the scene, only use one.");
        }
        instance = this;
    }

    public void NewGame(){
        this.gameData = new GameData();
    }
    public void LoadGame(){
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
    }
    private void Start(){
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
