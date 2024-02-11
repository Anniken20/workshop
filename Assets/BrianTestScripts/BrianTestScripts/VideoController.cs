using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public string nextSceneName; // Name of the scene to transition to
    public VideoPlayer videoPlayer; 
    public RawImage rawImage; 
    private bool canSkip = false; 

    void Start()
    {
        // Subscribe to the videoPlayer loopPointReached event
        videoPlayer.loopPointReached += EndReached;

        // Play the video
        videoPlayer.Play();
    }

    void Update()
    {
        // Check for input to enable skipping
        if (Input.GetKeyDown(KeyCode.Space) && canSkip)
        {
            SkipVideo();
        }
    }

    void EndReached(VideoPlayer vp)
    {
        // Load the next scene when the video ends
        SceneManager.LoadScene("Level 1");
    }

    void SkipVideo()
    {
        // Load "Level 1" scene when skipping
        SceneManager.LoadScene("Level 1");
    }

    void OnDestroy()
    {
        // Unsubscribe from the event
        videoPlayer.loopPointReached -= EndReached;
    }
    
    public void EnableSkipping()
    {
        canSkip = true;
    }
}

