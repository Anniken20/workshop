using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BountyBoard : MonoBehaviour, IDataPersistence
{
    public GameObject levelSelectPopup;
    public LevelManager levelManager;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button backButton;
    //public GameObject particleEffect;
    // public ClearSaveData clearSaveDataScript;

    public bool playerEnteredOnce = false;
    // private string playerEnteredOnceKey = "PlayerEnteredOnce";

    // Save and load 
    public bool calltoactionCompleted = false;

    public bool popupActivated = false;

    public Collider collider1;
    private int levelCompleted;
    //Cutscene Stuff
    public VideoController videoController;
    public GameObject videoCanvas;
    public float videoLength;
    public GameObject playerHUD;
    public GameObject hubMusic;

    private void Start()
    {
        // Don't use player prefs
        // Load playerEnteredOnce state from PlayerPrefs
        // playerEnteredOnce = PlayerPrefs.GetInt(playerEnteredOnceKey, 0) == 1;
        //playerEnteredOnce = false;
        // Buttons to their respective functions
        /*level1Button.onClick.AddListener(StartLevel1);
        level2Button.onClick.AddListener(StartLevel2);
        level3Button.onClick.AddListener(StartLevel3);*/
        backButton.onClick.AddListener(BackButtonClicked);

        // Deactivate the level select pop-up initially
        levelSelectPopup.SetActive(false);

        // Enable the particle effect when the scene starts
        //particleEffect.SetActive(true);

        if(levelCompleted == 0)
        {
            level2Button.interactable = false;
            level3Button.interactable = false;
            level2Button.gameObject.SetActive(false);
            level3Button.gameObject.SetActive(false);
        }
        // Check if level 1 is completed
        if (levelCompleted >= 1)
        {
            // Enable level 2 button if level 1 is completed
            level2Button.interactable = true;
        }
        else
        {
            // If level 1 is not completed, hide level 2 and 3 buttons
            SetButtonVisibility(level2Button, false);
            SetButtonVisibility(level3Button, false);
        }

        // Check if level 2 is completed
        if (levelCompleted >= 2)
        {
            // Enable level 3 button if level 2 is completed
            level3Button.interactable = true;
        }
        else
        {
            // If level 2 is not completed, hide level 3 button
            SetButtonVisibility(level3Button, false);
        }
    }

    private void SetButtonVisibility(Button button, bool isVisible)
    {
        if (button != null)
        {
            button.gameObject.SetActive(isVisible);

            // If the button has an Image component, disable it as well
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.enabled = isVisible;
            }
        }
    }

    private void OnTriggerEnter(Collider collider1)
    {
        if (collider1.CompareTag("Player") && !popupActivated)
        {
            if (!playerEnteredOnce)
            {
                //calltoactionCompleted = false;
                // First time player enters the trigger, move to CallToActionCutscene_MP4 scene
                playerEnteredOnce = true;
                FindObjectOfType<DataManager>().SaveGame();
                //SceneManager.LoadScene("CallToActionCutscene_MP4");
                if (videoController != null)
                {
                    hubMusic.SetActive(false);
                    playerHUD.SetActive(false);
                    videoCanvas.SetActive(true);
                    videoController.Start();
                    AudioManager.main.musicAudio.Stop();
                }
                PauseMenu.main.UnPauseNoUI(); // Unpause the game when the pop-up is deactivated
                //PlayerPrefs.SetInt(playerEnteredOnceKey, 1); // Save playerEnteredOnce state to PlayerPrefs
                playerEnteredOnce = true;
                // Disable the particle effect when transitioning to the next scene
                //particleEffect.SetActive(false);
            }
            else
            {
                PauseMenu.main.PauseNoUI(); // Pause the game when the pop-up is active
                //calltoactionCompleted = true;
                // Player has already entered once, show the level select pop-up
                levelSelectPopup.SetActive(true);

                // Disable the particle effect when showing the pop-up
                //particleEffect.SetActive(false);
            }
            popupActivated = true; // Set popupActivated to true to prevent multiple activations
        }
    }

    private void OnTriggerExit(Collider collider1)
    {
        if (collider1.CompareTag("Player"))
        {
            PauseMenu.main.UnPauseNoUI(); // Unpause the game when the pop-up is deactivated
            // Player exited proximity, hide the level select pop-up
            levelSelectPopup.SetActive(false);
            popupActivated = false;
        }
    }

    // Functions to start each level
    private void StartLevel1()
    {
        PauseMenu.main.UnPauseNoUI(); // Unpause the game when starting Level 3
        // Load Level 1
        SceneManager.LoadScene("Level 1");
        Debug.Log("Starting Level 1...");
    }

    private void StartLevel2()
    {
        PauseMenu.main.UnPauseNoUI(); // Unpause the game when starting Level 3
        // Load Level 2
        SceneManager.LoadScene("Level 2");
        Debug.Log("Starting Level 2...");
    }

    private void StartLevel3()
    {
        PauseMenu.main.UnPauseNoUI(); // Unpause the game when starting Level 3
        // Load Level 3
        SceneManager.LoadScene("Level 3");
        Debug.Log("Starting Level 3...");
    }

    // Method to handle back button click event
    private void BackButtonClicked()
    {
        // Check if the player is still inside the trigger area
        if (!IsPlayerInsideTriggerArea())
        {
            PauseMenu.main.UnPauseNoUI(); // Unpause the game when the pop-up is deactivated
            // Deactivate the level select pop-up
            levelSelectPopup.SetActive(false);
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


    /* Method to clear player's data
    public void ClearPlayerData()
    {
        if (clearSaveDataScript != null)
        {
            clearSaveDataScript.Pressed();
        }
    }*/

    public void LoadData(GameData data)
    {
        //this.calltoactionCompleted = data.calltoactionCompleted;
        //Debug.Log("Loading");
        this.playerEnteredOnce = data.calltoactionCompleted;
        levelCompleted = data.levelComplete;
    }

    public void SaveData(ref GameData data)
    {
        //data.calltoactionCompleted = this.calltoactionCompleted;
        data.calltoactionCompleted = this.playerEnteredOnce;
    }

    public void EndVideo()
    {
        if(videoCanvas != null)
        {
            videoCanvas.SetActive(false);
            hubMusic.SetActive(true);
        }
    }
}