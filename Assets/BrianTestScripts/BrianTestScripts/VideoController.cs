using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    public bool isIntro;

    void Start()
    {
        // Subscribe to the videoPlayer loopPointReached event
        videoPlayer.loopPointReached += EndReached;

        // Play the video
        videoPlayer.Play();

        // Enable skipping after a short delay
        StartCoroutine(EnableSkippingDelay());
    }

    IEnumerator EnableSkippingDelay()
    {
        yield return new WaitForSeconds(1f); // Adjust the delay time as needed
        canSkip = true;
    }

    void Update()
    {
        // Check for input to enable skipping
        if (canSkip && Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }

    void EndReached(VideoPlayer vp)
    {
        // Load the next scene when the video ends
        LoadScene(nextSceneName);
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
        // Load the next scene when skipping
        LoadScene(nextSceneName);
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

    }
}
