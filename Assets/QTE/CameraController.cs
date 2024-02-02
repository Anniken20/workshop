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
    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera shoulderCam;

    private bool isIsometricView = false;

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
}