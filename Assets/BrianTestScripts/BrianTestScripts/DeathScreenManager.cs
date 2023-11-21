using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathScreenManager : MonoBehaviour
{
    public GameObject deathScreen;

    private bool isDeathScreenActive = false;

    private void Update()
    {
        if (isDeathScreenActive)
        {
            Cursor.visible = true; // Show the cursor when the death screen is active
            Cursor.lockState = CursorLockMode.None; // Allow the cursor to move freely
        }
        else
        {
            Cursor.visible = false; // Hide the cursor during gameplay
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor during gameplay
        }
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        isDeathScreenActive = true;
        Time.timeScale = 0f; // Pause the game
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
        isDeathScreenActive = false;
        Time.timeScale = 1f; // Resume the game
    }

    public void RestartFromCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
