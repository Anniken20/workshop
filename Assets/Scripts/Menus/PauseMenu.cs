using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject settingsPanel;
    public GameObject controlsPanel;
    public GameObject mainMenu;
    public GameObject HUD;

    public Image fadeImage; // Reference to the fade image object
    public float fadeSpeed; // Speed of the fade effect

    //delegate events that every script can subscribe to.
    //when this is called via this script, every script's subscriber function is called
    public delegate void OnPause();
    public static event OnPause onPause;

    public delegate void OnResume();
    public static event OnResume onResume;

    [HideInInspector] public static bool paused;

    private const string mainMenuScene = "MainMenu";

    private InputAction pause;
    private InputAction cotnrols;
    public CharacterMovement iaControls;

    private float prevTimeScale;

    //singleton
    public static PauseMenu main;

    private void Awake()
    {
        iaControls = new CharacterMovement();
        main = this;
        prevTimeScale = 1f;
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
        if(Time.timeScale != 0) prevTimeScale = Time.timeScale;

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
        Time.timeScale = prevTimeScale;
        onResume?.Invoke();

        // Activate the fade image
        fadeImage.gameObject.SetActive(true);

        // Start the fade effect
        StartCoroutine(FadeOutImage());
    }

    public void PauseNoUI()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (Time.timeScale != 0) prevTimeScale = Time.timeScale;

        Time.timeScale = 0;
        onPause?.Invoke();
    }

    public void UnPauseNoUI()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = prevTimeScale;
        onResume?.Invoke();
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
        controlsPanel.SetActive(false);
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
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private IEnumerator FadeOutImage()
    {
        Color color = fadeImage.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }

        // Deactivate the fade image when the fade is complete
        fadeImage.gameObject.SetActive(false);
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
