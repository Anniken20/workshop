using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject settingsPanel;
    public GameObject mainMenu;

    //delegate events that every script can subscribe to.
    //when this is called via this script, every script's subscriber function is called
    public delegate void OnPause();
    public static event OnPause onPause;

    public delegate void OnResume();
    public static event OnResume onResume;

    [HideInInspector] public static bool paused;

    private const string mainMenuScene = "MainMenu";

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
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
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        onPause?.Invoke();
    }
    public void Continue()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        onResume?.Invoke();
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
