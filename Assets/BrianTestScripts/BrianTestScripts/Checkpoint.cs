using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class Checkpoint : MonoBehaviour, IDataPersistence
{
    private static List<Checkpoint> checkpoints = new List<Checkpoint>();
    private static Checkpoint activeCheckpoint;
    private string checkpointScene;
    private Vector3 savedCheckpoint;
    private DataManager dm;

    private void Start()
    {
        checkpoints.Add(this);
        dm =  FindObjectOfType<DataManager>();
        LoadToCheckpoint();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().name != "HUB" && SceneManager.GetActiveScene().name != "Tutorial 1"){
                // Set this checkpoint as the active checkpoint
                activeCheckpoint = this;
                checkpointScene = SceneManager.GetActiveScene().name;
                dm.SaveGame();
            }
        }
    }

    public static Checkpoint GetActiveCheckpoint()
    {
        return activeCheckpoint;
    }

    public static List<Checkpoint> GetCheckpoints()
    {
        return checkpoints;
    }
    public void LoadData(GameData data){
        //Debug.Log("Loading Data...");
        this.savedCheckpoint = data.savedCheckpoint;
        this.checkpointScene = data.checkpointScene;
        CoinCollector.coinsCollected = data.coins;
        if(data.checkpointScene != "HUB" && data.checkpointScene != "Tutorial 1"){
            LoadToCheckpoint();
        }
        
    }
    public void SaveData(ref GameData data){
        if(activeCheckpoint != null){
            data.savedCheckpoint = activeCheckpoint.gameObject.transform.position;
            data.checkpointScene = SceneManager.GetActiveScene().name;
            data.coins = CoinCollector.coinsCollected;
        }
    }
    private void LoadToCheckpoint(){
        //Debug.Log("Loading to checkpoint");
        if(savedCheckpoint != Vector3.zero && SceneManager.GetActiveScene().name == checkpointScene){
            //Debug.Log("Checkpoint exists and scene matches");
            //ThirdPersonController.Main.gameObject.transform.position = savedCheckpoint;
            //Camera.main.GetComponent<CameraController>().RecomposeCamera();
            ThirdPersonController.Main.InstantTeleport(savedCheckpoint);
            //Debug.Log("Loading to checkpoint in scene: " + checkpointScene + "Currently current scene: " + SceneManager.GetActiveScene().name);
        }
        else{
            //Debug.LogWarning("Something went wrong");
            //Debug.Log("Saved scene: " +checkpointScene +" - Current Scene: " +SceneManager.GetActiveScene().name);
        }
    }
}


