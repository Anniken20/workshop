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

    private void Start()
    {
        checkpoints.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set this checkpoint as the active checkpoint
            activeCheckpoint = this;
            checkpointScene = SceneManager.GetActiveScene().name;
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
        this.savedCheckpoint = data.savedCheckpoint;
        this.checkpointScene = data.checkpointScene;
        LoadToCheckpoint();
    }
    public void SaveData(ref GameData data){
        if(activeCheckpoint != null){
            data.savedCheckpoint = activeCheckpoint.gameObject.transform.position;
            data.checkpointScene = SceneManager.GetActiveScene().name;
        }
    }
    private void LoadToCheckpoint(){
        if(savedCheckpoint != Vector3.zero && SceneManager.GetActiveScene().name == checkpointScene){
            //Debug.Log("Checkpoint exists and scene matches");
            ThirdPersonController.Main.gameObject.transform.position = savedCheckpoint;
            Camera.main.GetComponent<CameraController>().RecomposeCamera();
        }
        else{
            //Debug.LogWarning("Something went wrong");
            //Debug.Log("Saved scene: " +checkpointScene +" - Current Scene: " +SceneManager.GetActiveScene().name);
        }
    }
}


