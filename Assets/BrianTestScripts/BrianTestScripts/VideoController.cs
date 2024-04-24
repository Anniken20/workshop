using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;
using StarterAssets;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro.Examples;

public class VideoController : MonoBehaviour, IDataPersistence
{
    //public CharacterMovement iaControls;
    //private InputAction shoot;
    public GameObject LoadingScreen;
    public Image LoadingBarFill;
    public string nextSceneName; // Name of the scene to transition to
    public VideoPlayer videoPlayer; 
    public RawImage rawImage; 
    private bool canSkip = false; 
    public bool skipped = false;
    public bool isIntro;
    public bool introCompleted;
    public DataManager dataManager;
    bool triggeredOnce = true;
    public QTEDuel qteDuel;
    public BountyBoard bountyBoard;

    public void Start()
    {
        // Subscribe to the videoPlayer loopPointReached event
        videoPlayer.loopPointReached += EndReached;

        // Play the video
        videoPlayer.Play();
        ThirdPersonController.Main.ForceStartConversation();
        ThirdPersonController.Main.gameObject.GetComponent<PlayerHealth>().invulnerable = true;
        // Enable skipping after a short delay
        StartCoroutine(EnableSkippingDelay());
    }

    IEnumerator EnableSkippingDelay()
    {
        yield return new WaitForSeconds(1f); // Adjust the delay time as needed
        canSkip = true;
        Debug.Log("Can Skip");
    }

    void Update()
    {
        // Check for input to enable skipping
        if (canSkip && Input.GetKeyDown(KeyCode.Space))
        {
            skipped = true;
            SkipVideo();
        }
        if(isIntro && introCompleted && triggeredOnce){
            triggeredOnce = false;
            SkipVideo();

        }
    }

    void EndReached(VideoPlayer vp)
    {
        if(isIntro){
            introCompleted = true;
            Debug.Log(introCompleted);
            if(dataManager != null)
                {
                    dataManager.SaveGame();
                }
        }
        // Load the next scene when the video ends
        if (nextSceneName != null)
            LoadScene(nextSceneName);
        if (bountyBoard != null)
        {
            bountyBoard.EndVideo();
        }
    }

    public void LoadScene(string nextSceneName)
    {
        StartCoroutine(LoadSceneAsync(nextSceneName));
    }

    IEnumerator LoadSceneAsync(string nextSceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue=Mathf.Clamp01(operation.progress/0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }

    void SkipVideo()
    {
        if(isIntro){
            // Load the next scene when skipping
            introCompleted = true;
            Debug.Log(introCompleted);
            if(dataManager != null)
                {
                    dataManager.SaveGame();
                }
        }
    
        LoadScene(nextSceneName);

        if (qteDuel != null)
        {
            ThirdPersonController.Main.ForceStopConversation();
            ThirdPersonController.Main.gameObject.GetComponent<PlayerHealth>().invulnerable = false;
            AudioManager.main.musicAudio.Play();
            qteDuel.PlayerWonDuel();
            qteDuel.EndDuel();
        }
        if (bountyBoard != null)
        {
            ThirdPersonController.Main.ForceStopConversation();
            AudioManager.main.musicAudio.Play();
            bountyBoard.EndVideo();
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event
        videoPlayer.loopPointReached -= EndReached;
    }

    /*private void awake()
    {
        iaControls = new CharacterMovement();
    }

    private void OnEnable()
    {
        shoot = iaControls.CharacterControls.shootInput;
        shoot.Enable();
    }

    private void OnDisable()
    {
        shoot.Disable();
    }*/
    public void LoadData(GameData data){
        introCompleted = data.introCompleted;
        if(data.checkpointScene != "" && isIntro || data.checkpointScene != null && isIntro){
            this.nextSceneName = data.checkpointScene;
            //Debug.Log("next scene set to: " +data.checkpointScene +" Confirmation: " +nextSceneName);
        }
        else{
            //nextSceneName = "Tutorial 1";
            //Debug.Log("next scene set to da tutorial " +" Confirmation: " +nextSceneName);
        }
    }
    public void SaveData(ref GameData data){
        //Debug.Log("Saving Data");
        data.introCompleted = introCompleted;
    }
}
