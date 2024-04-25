using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro.Examples;

public class SceneTeleport : MonoBehaviour, IDataPersistence
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    public string scene_name;
    public DataManager dataManager;
    public int levelCompleted;
    private int savedComplete;
    public bool isHubTeleport;
    private bool ranInto;
    public void Start(){
        if(dataManager != null)
            {
                dataManager.LoadGame();

            }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(dataManager != null)
            {
                this.ranInto = true;
                dataManager.SaveGame();
                LoadScene(scene_name);
                Debug.Log("loaded scene");
            }

        } 
    }
    
    public void LoadData(GameData data){
        savedComplete = data.levelComplete;
        //Debug.Log("Saved Complete: " +savedComplete);
        //Debug.Log("Stored Data Complete: " +data.levelComplete);
    }
    public void SaveData(ref GameData data){
        //Debug.Log("Saving");
        if(this.levelCompleted >= savedComplete && this.levelCompleted != 0){
            if(this.isHubTeleport && ranInto == true){
            Debug.Log("Doing thing");
            data.levelComplete = this.levelCompleted;
            }
        }
    }

    public void LoadScene(string scene_name)
    {
        StartCoroutine(LoadSceneAsync(scene_name));
    }

    IEnumerator LoadSceneAsync(string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue=Mathf.Clamp01(operation.progress/0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
}
