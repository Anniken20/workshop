using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject settingsPanel;
    public GameObject mainMenu;
    public GameObject HUD;

    //delegate events that every script can subscribe to.
    //when this is called via this script, every script's subscriber function is called
    public delegate void OnPause();
    public static event OnPause onPause;

    public delegate void OnResume();
    public static event OnResume onResume;

    [HideInInspector] public static bool paused;

    private const string mainMenuScene = "MainMenu";

    private InputAction pause;
    public CharacterMovement iaControls;

    //singleton
    public static PauseMenu main;

    private void Awake()
    {
        iaControls = new CharacterMovement();
        main = this;
    }

    private void Update()
    {
        if(pause.triggered)
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
        HUD.SetActive(false);
        Time.timeScale = 0;
        onPause?.Invoke();
    }
    public void Continue()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePanel.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1;
        onResume?.Invoke();
    }

    public void PauseNoUI()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void UnPauseNoUI()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    public void Restart()
    {
        Continue();
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        PausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        onResume?.Invoke();

        SceneManager.LoadScene(mainMenuScene);

        //temporary fix
        //Application.Quit();
    }

    //when not in-game but looking at settings
    public void Back()
    {
        if(mainMenu != null) mainMenu.SetActive(true);
        if(PausePanel != null) PausePanel.SetActive(false);
    }

    public void LeaveSettings()
    {
        settingsPanel.SetActive(false);
        if(PausePanel != null) PausePanel.SetActive(true);
    }

    private void OnEnable()
    {
        pause = iaControls.CharacterControls.Pause;

        pause.Enable();
    }
    private void OnDisable()
    {
        pause.Disable();
    }
}
