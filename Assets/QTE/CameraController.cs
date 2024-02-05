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

    private float aimDZWidth;
    private float aimDZHeight;

    private float bodyYDamping;

    private void Start()
    {
        //save scene settings
        CinemachineComponentBase componentBase = mainCam.GetCinemachineComponent(CinemachineCore.Stage.Aim);
        if(componentBase is CinemachineComposer composer)
        {
            aimDZWidth = composer.m_DeadZoneWidth;
            aimDZHeight = composer.m_DeadZoneHeight;
        }

        CinemachineComponentBase componentBaseB = mainCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if(componentBaseB is CinemachineTransposer transposer){
            bodyYDamping = transposer.m_YDamping;
        }
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

    public void SwitchToTeleportMode(bool yes = true)
    {
        CinemachineComponentBase componentBase = mainCam.GetCinemachineComponent(CinemachineCore.Stage.Aim);

        if (componentBase is CinemachineComposer composer)
        {
            if (yes)
            {
                composer.m_DeadZoneHeight = 0;
                composer.m_DeadZoneWidth = 0;
                
                CinemachineComponentBase componentBaseB = mainCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
                if (componentBaseB is CinemachineTransposer transposer)
                {
                    transposer.m_YDamping = 0;
                }
            }
            else
            {
                composer.m_DeadZoneHeight = aimDZWidth;
                composer.m_DeadZoneWidth = aimDZHeight;

                CinemachineComponentBase componentBaseB = mainCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
                if (componentBaseB is CinemachineTransposer transposer)
                {
                    transposer.m_YDamping = bodyYDamping;
                }
            }
        }
    }
}