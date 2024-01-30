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
 * Changed to fit QTESys 1/27/24
 */
public class CameraController : MonoBehaviour
{
    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera shoulderCam;

    private bool isIsometricView = false;

    public void SwitchCameraView()
    {
        // Flip isometric bool
        isIsometricView = !isIsometricView;

        // Change Cinemachine virtual cam priorities.
        // Cinemachine brain will automatically "blend" to the highest priority camera.
        if (isIsometricView)
        {
            // Change priorities
            mainCam.Priority = 0;
            shoulderCam.Priority = 1;
        }
        else
        {
            // Change priorities
            mainCam.Priority = 1;
            shoulderCam.Priority = 0;
        }
    }
}