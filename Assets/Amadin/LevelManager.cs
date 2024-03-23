using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // Singleton instance

    private List<string> completedLevels = new List<string>(); // List to store completed levels

    public GameObject[] levelNPCs; // Array of NPCs representing each level

    // Make sure NPC GameObject names in Unity match the level names: "Level 1 NPC", "Level 2 NPC", "Level 3 NPC"
    // Added a lot of debug logs here for you to test whatever npc and level you decide to complete
    // It will save the PlayerPrefs so when you close the game it will save it, idk if there's a different method of saving it right now - but bandaid fix

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        LoadCompletedLevels(); // Load completed levels from PlayerPrefs
        UpdateHubNPCs(); // Update NPC states in the hub
    }

    // Mark a level as completed
    public void MarkLevelComplete(string levelName)
    {
        if (!completedLevels.Contains(levelName))
        {
            completedLevels.Add(levelName);
            SaveCompletedLevels(); // Save updated list to PlayerPrefs
            UpdateHubNPCs(); // Update NPC states in the hub
            Debug.Log("Level completed: " + levelName);
        }
        else
        {
            Debug.LogWarning("Level " + levelName + " is already completed!");
        }
    }

    // Check if a level is completed
    public bool IsLevelCompleted(string levelName)
    {
        return completedLevels.Contains(levelName);
    }

    // Save completed levels to PlayerPrefs
    private void SaveCompletedLevels()
    {
        PlayerPrefs.SetString("CompletedLevels", string.Join(",", completedLevels.ToArray()));
        PlayerPrefs.Save();
        Debug.Log("Completed levels saved.");
    }

    // Load completed levels from PlayerPrefs
    private void LoadCompletedLevels()
    {
        string levels = PlayerPrefs.GetString("CompletedLevels", "");
        if (!string.IsNullOrEmpty(levels))
        {
            completedLevels.AddRange(levels.Split(','));
            Debug.Log("Completed levels loaded: " + levels);
        }
        else
        {
            Debug.Log("No completed levels found.");
        }
    }

    // Update NPC states in the hub based on completed levels
    private void UpdateHubNPCs()
    {
        for (int i = 1; i < levelNPCs.Length; i++) // Start from index 1 to skip Tutorial
        {
            string levelName = "Level " + i; // Level names: Level 1, Level 2, Level 3
            bool isUnlocked = completedLevels.Contains(levelName);
            levelNPCs[i].SetActive(isUnlocked); // Activate NPC if the corresponding level is completed
            Debug.Log("NPC " + levelName + " state: " + isUnlocked);
        }
    }
}
