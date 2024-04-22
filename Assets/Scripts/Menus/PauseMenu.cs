using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using StarterAssets;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject settingsPanel;
    //public GameObject controlsPanel;
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
    private InputAction controls;
    public CharacterMovement iaControls;

    private float prevTimeScale;

    //singleton
    public static PauseMenu main;

    // New panels
    public GameObject AccessibilityPanel;
    public GameObject PcPanel;
    public GameObject ControllerPanel;
    public GameObject ResolutionPanel;

    private GameObject activePanel;

    private void Awake()
    {
        iaControls = new CharacterMovement();
        main = this;
        prevTimeScale = 1f;
    }

    private void Start()
    {
        // Disable the fade image at the start of the game
        fadeImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (pause.triggered)
        {
            if (paused)
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
        //Debug.Log("paused");
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PausePanel.SetActive(true);
        HUD.SetActive(false);
        if (Time.timeScale != 0) prevTimeScale = Time.timeScale;

        Time.timeScale = 0;
        onPause?.Invoke();
    }
    public void Continue()
    {
        //Debug.Log("Resume"); // Check if the method is being called

        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePanel.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = prevTimeScale;

        onResume?.Invoke();
        SubSettingBack();
        settingsPanel.SetActive(false);

        // Enable the canvas containing the fade image
        Canvas canvas = fadeImage.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;
        }
        else
        {
            Debug.LogError("Canvas not found for fade image!");
        }

        // Enable the fade image
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
        AccessibilityPanel.SetActive(false);
        PcPanel.SetActive(false);
        ControllerPanel.SetActive(false);
        ResolutionPanel.SetActive(false);
        //controlsPanel.SetActive(false);
    }

    public void ActivateAccessibilityPanel()
    {
        Debug.Log("Showing Accessibility Controls");
        AccessibilityPanel.SetActive(true);
        PcPanel.SetActive(false);
        ControllerPanel.SetActive(false);
        ResolutionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        PausePanel.SetActive(false);

        activePanel = AccessibilityPanel;
    }

    public void ActivatePcPanel()
    {
        Debug.Log("Showing PC Controls");
        AccessibilityPanel.SetActive(false);
        PcPanel.SetActive(true);
        ControllerPanel.SetActive(false);
        ResolutionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        PausePanel.SetActive(false);

        activePanel = PcPanel;
    }

    public void ActivateControllerPanel()
    {
        Debug.Log("Showing Controller Controls");
        AccessibilityPanel.SetActive(false);
        PcPanel.SetActive(false);
        ControllerPanel.SetActive(true);
        ResolutionPanel.SetActive(false);
        settingsPanel.SetActive(false);
        PausePanel.SetActive(false);

        activePanel = ControllerPanel;
    }


    public void ActivateResolutionPanel()
    {
        Debug.Log("Showing Resolution Controls");
        AccessibilityPanel.SetActive(false);
        PcPanel.SetActive(false);
        ControllerPanel.SetActive(false);
        ResolutionPanel.SetActive(true);
        settingsPanel.SetActive(false);
        PausePanel.SetActive(false);

        activePanel = ResolutionPanel;
    }

    public void SubSettingBack()
    {
        if(activePanel != null) activePanel.SetActive(false);
        activePanel = null;
        settingsPanel.SetActive(true);
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

    /*public void ShowControls()
    {
        Debug.Log("Showing Controls");
        controlsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }*/

    private IEnumerator FadeOutImage()
    {
        // Ensure fade image is active
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            //Debug.Log("Alpha: " + color.a); // Check alpha value during fade
            yield return null;
        }

        // Set the alpha back to 1 after fading out
        color.a = 1f;
        fadeImage.color = color;

        // Deactivate the fade image when the fade is complete
        fadeImage.gameObject.SetActive(false);
    }

    //when not in-game but looking at settings
    public void Back()
    {
        if (mainMenu != null) mainMenu.SetActive(true);
        if (PausePanel != null) PausePanel.SetActive(false);
    }

    /*public void LeaveSettings()
    {
        Debug.Log("left settings");
        settingsPanel.SetActive(false);
        if (PausePanel != null) PausePanel.SetActive(true);
    }*/

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
