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
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(dataManager != null)
            {
                dataManager.SaveGame();
            }
            LoadScene(scene_name);
        } 
    }
    public int levelCompleted;
    public void LoadData(GameData data){
        //levelCompleted = data.levelComplete;
    }
    public void SaveData(ref GameData data){
        data.levelComplete = levelCompleted;
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
