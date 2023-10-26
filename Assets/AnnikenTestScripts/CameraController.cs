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
 */

public class CameraController : MonoBehaviour
{
    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera shoulderCam;

    public float switchViewDuration;

    private bool isIsometricView = false;

    private void Update()
    {
        // Button = switch
        if (Input.GetKeyDown(KeyCode.C))
        {
            //flip isometric bool
            isIsometricView = !isIsometricView;

            //change cinemachine virtual cam priorities.
            //cinemachine brain will automatically "blend" to the highest priority camera.
            if (isIsometricView)
            {
                //set transition time of cinemachine camera blend
                camBrain.m_DefaultBlend.m_Time = switchViewDuration;

                //change priorities
                mainCam.Priority = 0;
                shoulderCam.Priority = 1;
            } else
            {
                //set transition time of cinemachine camera blend
                camBrain.m_DefaultBlend.m_Time = switchViewDuration;

                //change priorities
                mainCam.Priority = 1;
                shoulderCam.Priority = 0;
            }
        }
    }
}