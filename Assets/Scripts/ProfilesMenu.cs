using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfilesMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject profilesMenu;
    public GameObject controlsMenu;

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
        selectedProfileIndex = profileIndex;

        // Check if any profile exists
        if (PlayerPrefs.HasKey("SelectedProfileIndex"))
        {
            // Check if the player has previously visited a scene
            if (PlayerPrefs.HasKey("LastVisitedScene_" + selectedProfileIndex))
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
            }
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
        // Mark the intro cutscene as completed for the selected profile
        PlayerPrefs.SetInt("IntroCutsceneCompleted_" + selectedProfileIndex, 1);
        // Unload the current scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        // Start the game scene
        StartGameWithProfile(selectedProfileIndex);
    }

    // Call this method when the player transitions to a new scene
    public void SceneTransition(string sceneName)
    {
        // Store the current scene as the last visited scene for the selected profile
        PlayerPrefs.SetString("LastVisitedScene_" + selectedProfileIndex, sceneName);
    }
}