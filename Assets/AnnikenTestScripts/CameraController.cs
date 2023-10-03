using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform isometricCameraTransform; // The isometric camera position and rotation.
    public Transform overShoulderCameraTransform; // The over-the-shoulder camera position and rotation.
    public float cameraTransitionSpeed = 2.0f; // Speed of camera transition.
    private bool isometricView = true; // Flag to track the camera view.

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the camera in isometric view.
        SwitchToIsometricView();
    }

    // Method to switch to the isometric camera view.
    public void SwitchToIsometricView()
    {
        isometricView = true;
    }

    // Method to switch to the over-the-shoulder camera view.
    public void SwitchToOverShoulderView()
    {
        isometricView = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Transition the camera position and rotation based on the current view.
        Transform targetTransform = isometricView ? isometricCameraTransform : overShoulderCameraTransform;
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * cameraTransitionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, Time.deltaTime * cameraTransitionSpeed);
    }
}