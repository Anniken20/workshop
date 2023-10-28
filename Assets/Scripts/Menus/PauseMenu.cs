using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PausePanel;
    public GameObject settingsPanel;
    public GameObject mainMenu;

    private const string mainMenuScene = "MainMenu";

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    public void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    //when not in-game but looking at settings
    public void Back()
    {
        if(mainMenu != null) mainMenu.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void LeaveSettings()
    {
        settingsPanel.SetActive(false);
        PausePanel.SetActive(true);
    }
}
