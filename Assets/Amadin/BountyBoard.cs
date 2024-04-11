using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BountyBoard : MonoBehaviour
{
    public GameObject levelSelectPopup;
    public LevelManager levelManager;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backButton;
    public GameObject particleEffect;
    public ClearSaveData clearSaveDataScript;

    private bool playerEnteredOnce = false;
    private string playerEnteredOnceKey = "PlayerEnteredOnce";
    private bool paused = false;
    private float prevTimeScale = 1f;
    public static event Action onPause;

    // Save data keys for defeating characters
    private string carilloDefeatedKey = "CarilloDefeated";
    private string santanaDefeatedKey = "SantanaDefeated";
    private string dianaDefeatedKey = "DianaDefeated";

    public bool popupActivated = false;

    public Collider collider1;

    private void Start()
    {
        // Load playerEnteredOnce state from PlayerPrefs
        playerEnteredOnce = PlayerPrefs.GetInt(playerEnteredOnceKey, 0) == 1;

        // Buttons to their respective functions
        /*level1Button.onClick.AddListener(StartLevel1);
        level2Button.onClick.AddListener(StartLevel2);
        level3Button.onClick.AddListener(StartLevel3);*/
        backButton.onClick.AddListener(BackButtonClicked);

        // Subscribe to onPause event
        PauseMenu.onPause += PauseNoUI;

        // Deactivate the level select pop-up initially
        levelSelectPopup.SetActive(false);

        // Enable the particle effect when the scene starts
        particleEffect.SetActive(true);

        //Check if characters are defeated and activate/deactivate level buttons accordingly
        bool carilloDefeated = PlayerPrefs.GetInt(carilloDefeatedKey, 0) == 1;
        bool santanaDefeated = PlayerPrefs.GetInt(santanaDefeatedKey, 0) == 1;
        bool dianaDefeated = PlayerPrefs.GetInt(dianaDefeatedKey, 0) == 1;

        // Enable level 2 button if Carillo is defeated
        //level2Button.interactable = carilloDefeated;

        // Enable level 3 button if Santana is defeated
        //level3Button.interactable = santanaDefeated;

        // Disable level 2 and level 3 buttons if Carillo and Santana are not defeated respectively
        if (levelManager.level < 1)
        {
            level2Button.interactable = false;
            level3Button.interactable = false;
        }
        else if (levelManager.level < 2)
        {
            level3Button.interactable = false;
        }

        if (levelManager.level == 1)
        {
            level2Button.interactable = true;
            level3Button.interactable = false;
        }
        else if (levelManager.level == 2)
        {
            level2Button.interactable = true;
            level3Button.interactable = true;
        }
    }

    private void OnTriggerEnter(Collider collider1)
    {
        if (collider1.CompareTag("Player") && !popupActivated)
        {
            if (!playerEnteredOnce)
            {
                // First time player enters the trigger, move to CallToActionCutscene_MP4 scene
                SceneManager.LoadScene("CallToActionCutscene_MP4");
                playerEnteredOnce = true;
                PlayerPrefs.SetInt(playerEnteredOnceKey, 1); // Save playerEnteredOnce state to PlayerPrefs
                UnPauseNoUI(); // Pause the game when the scene loads

                // Disable the particle effect when transitioning to the next scene
                particleEffect.SetActive(false);
            }
            else
            {
                // Player has already entered once, show the level select pop-up
                levelSelectPopup.SetActive(true);
                PauseNoUI(); // Pause the game when the pop-up is active

                // Disable the particle effect when showing the pop-up
                particleEffect.SetActive(false);
            }

            popupActivated = true; // Set popupActivated to true to prevent multiple activations
        }
    }

    private void OnTriggerExit(Collider collider1)
    {
        if (collider1.CompareTag("Player"))
        {
            // Player exited proximity, hide the level select pop-up
            levelSelectPopup.SetActive(false);
            UnPauseNoUI(); // Unpause the game when the pop-up is deactivated
            popupActivated = false;
        }
    }

    // Functions to start each level
    private void StartLevel1()
    {
        // Load Level 1
        SceneManager.LoadScene("Level 1");
        Debug.Log("Starting Level 1...");
        UnPauseNoUI(); // Unpause the game when starting Level 1
    }

    private void StartLevel2()
    {
        // Load Level 2
        SceneManager.LoadScene("Level 2");
        Debug.Log("Starting Level 2...");
        UnPauseNoUI(); // Unpause the game when starting Level 2
    }

    private void StartLevel3()
    {
        // Load Level 3
        SceneManager.LoadScene("Level 3");
        Debug.Log("Starting Level 3...");
        UnPauseNoUI(); // Unpause the game when starting Level 3
    }

    // Method to handle back button click event
    private void BackButtonClicked()
    {
        // Check if the player is still inside the trigger area
        if (!IsPlayerInsideTriggerArea())
        {
            // Deactivate the level select pop-up
            levelSelectPopup.SetActive(false);
            UnPauseNoUI(); // Unpause the game when the pop-up is deactivated

            // Unsubscribe from onPause event
            PauseMenu.onPause -= PauseNoUI;
        }
    }

    // Method to check if the player is inside the trigger area
    private bool IsPlayerInsideTriggerArea()
    {
        // Define the center and radius of the trigger area
        Vector3 center = transform.position;
        float radius = 2f; // 2-meter radius

        // Check if the player is inside the trigger area
        Collider[] colliders = Physics.OverlapSphere(center, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public void PauseNoUI()
    {
        if (!paused)
        {
            paused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Time.timeScale != 0) prevTimeScale = Time.timeScale;

            Time.timeScale = 0;
            onPause?.Invoke();
        }
    }

    public void UnPauseNoUI()
    {
        if (paused)
        {
            paused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = prevTimeScale;
            onPause?.Invoke();
        }
    }

    // Method to clear player's data
    public void ClearPlayerData()
    {
        if (clearSaveDataScript != null)
        {
            clearSaveDataScript.Pressed();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from onPause event
        PauseMenu.onPause -= PauseNoUI;
    }
}

