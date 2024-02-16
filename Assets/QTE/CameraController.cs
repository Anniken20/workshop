using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

/* Script for switching from isometric to other cameras
 * 
 * 
 * 
 * Anniken Bergo
 * Caden Henderson
 * 10/8/23
 * Changed to fit QTETest 1/27/24
 */

public class CameraController : MonoBehaviour
{
    public GameObject playerObject;
    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera shoulderCam;

    public Vector3 baseCameraOffset;
    private Vector3 cameraOffset;

    private void Start()
    {
        cameraOffset = mainCam.transform.position - playerObject.transform.position;
    }

    public void SwitchCameraView(bool switchToIsometric)
    {
        // Change Cinemachine virtual cam priorities.
        // Cinemachine brain will automatically "blend" to the highest priority camera.
        if (switchToIsometric)
        {
            // Switch to isometric view
            mainCam.Priority = 15;
            shoulderCam.Priority = 11;
        }
        else
        {
            // Switch to non-isometric view
            mainCam.Priority = 11;
            shoulderCam.Priority = 15;
        }
        //isIsometricView = !switchToIsometric;
    }

    public void RecomposeCamera()
    {
        //Debug.Log("Camera recomposed by offset: " + cameraOffset);
        
        StartCoroutine(ForceResetRoutine());
    }

    public void ResetCamera()
    {
        mainCam.transform.position = playerObject.transform.position + baseCameraOffset;
    }

    private IEnumerator ForceResetRoutine()
    {
        float duration = 2f;
        float endTime = Time.time + duration;
        while (true)
        {
            mainCam.transform.position = playerObject.transform.position + cameraOffset;
            if (Time.time > endTime) yield break;
            yield return null;
        }
    }

    public void SwitchToTeleportMode(bool yes = true)
    {
        
    }
}