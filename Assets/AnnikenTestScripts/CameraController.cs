using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera isometricCamera;
    public Camera thirdPersonCamera;

    private bool isIsometricView = true;

    private void Start()
    {
        // Which camera
        isometricCamera.enabled = isIsometricView;
        thirdPersonCamera.enabled = !isIsometricView;
    }

    private void Update()
    {
        // Button = switch
        if (Input.GetKeyDown(KeyCode.C))
        {
            isIsometricView = !isIsometricView;

            // Toggle camera iso and over shoulder
            isometricCamera.enabled = isIsometricView;
            thirdPersonCamera.enabled = !isIsometricView;
        }
    }
}