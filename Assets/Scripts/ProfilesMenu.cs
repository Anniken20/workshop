using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfilesMenu : MonoBehaviour, IDataPersistence
{
    public GameObject settingsMenu;
    public GameObject profilesMenu;
    public GameObject controlsMenu;
    private bool introCompleted;
    private string savedScene;
    private Vector3 savedCheckpoint;
    public DataManager dataManager;

    private int selectedProfileIndex;
    /// <summary>
    /// WHY THE FUCK DONT YOU FUCKING WORK YOU STUPID PIECE OF SHIT
    /// MAN FUCK YOU JUST WORK
    /// idiotic this entire god damn player prefs bs
    /// </summary>
    private void Start()
    {
        // Load the selected profile index if it's stored in PlayerPrefs
        if (PlayerPrefs.HasKey("SelectedProfileIndex"))
        {
            selectedProfileIndex = PlayerPrefs.GetInt("SelectedProfileIndex");
        }
    }

    public void StartGameWithProfile(int profileIndex)
    {
        if(dataManager != null){
            dataManager.LoadGame();
            Debug.Log("Force loading");

        }
        selectedProfileIndex = profileIndex;

        // Check if any profile exists
        //if (PlayerPrefs.HasKey("SelectedProfileIndex"))
        if(introCompleted)
        {
            SceneManager.LoadScene(savedScene);

            // Check if the player has previously visited a scene
            /*if (PlayerPrefs.HasKey("LastVisitedScene_" + selectedProfileIndex))
            {
                // Load the last visited scene for the selected profile
                string lastVisitedScene = PlayerPrefs.GetString("LastVisitedScene_" + selectedProfileIndex);
                SceneManager.LoadScene(lastVisitedScene);
            }
            else
            {
                Debug.LogWarning("Last visited scene not found. Loading default scene.");
                // Load a default scene if the last visited scene is not found
                SceneManager.LoadScene("Hub");
            }*/
        }
        else
        {
            // Load the intro cutscene if no profile exists
            SceneManager.LoadScene("IntroCutscene_MP4");
        }
    }

    // Call this method when the intro cutscene is completed
    public void IntroCutsceneCompleted()
    {
        introCompleted = true;
        // Mark the intro cutscene as completed for the selected profile
        //PlayerPrefs.SetInt("IntroCutsceneCompleted_" + selectedProfileIndex, 1);
        // Unload the current scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        // Start the game scene
        //StartGameWithProfile(selectedProfileIndex);
    }

    // Call this method when the player transitions to a new scene
    public void SceneTransition(string sceneName)
    {
        // Store the current scene as the last visited scene for the selected profile
        PlayerPrefs.SetString("LastVisitedScene_" + selectedProfileIndex, sceneName);
    }
    public void LoadData(GameData data){
        introCompleted = data.introCompleted;
        //savedCheckpoint = data.savedCheckpoint;
        savedScene = data.checkpointScene;
        Debug.Log("Is Completed: " +introCompleted);
    }

    public void SaveData(ref GameData data){
    }
}