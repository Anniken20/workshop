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

    void Start()
    {
        // Subscribe to the videoPlayer loopPointReached event
        videoPlayer.loopPointReached += EndReached;

        // Play the video
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        // Load the next scene when the video ends
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        // Unsubscribe from the event
        videoPlayer.loopPointReached -= EndReached;
    }
}

