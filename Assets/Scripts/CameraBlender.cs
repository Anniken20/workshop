using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/* Blend between different cinemachine virtual cameras
 * 
 * 
 * Caden Henderson
 * 11/16/23
 */

public class CameraBlender : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBrain brain;
    Camera mainCamComponent;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        brain = FindAnyObjectByType<CinemachineBrain>();
        mainCamComponent = brain.gameObject.GetComponent<Camera>();
    }

    public void ActivateCamera()
    {
        cam.Priority = 100;
    }

    public void DeactivateCamera()
    {
        cam.Priority = -1;
    }

    public void ChangeToPerspective()
    {
        mainCamComponent.orthographic = false;
    }

    public void ChangeToOrthographic()
    {
        mainCamComponent.orthographic = true;
    }

}
