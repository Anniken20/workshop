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

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ActivateCamera()
    {
        cam.Priority = 100;
    }

    public void DeactivateCamera()
    {
        cam.Priority = -1;
    }
}
