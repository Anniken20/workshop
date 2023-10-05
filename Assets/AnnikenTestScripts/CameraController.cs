using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public GameObject camera;
    public GameObject isometricCamPosition;
    public GameObject thirdPersonCamPosition;

    public float switchViewDuration;

    private bool isIsometricView = true;

    private void Start()
    {
        //align it with the proper place
        camera.transform.position = isometricCamPosition.transform.position;
    }

    private void Update()
    {
        // Button = switch
        if (Input.GetKeyDown(KeyCode.C))
        {
            isIsometricView = !isIsometricView;

            // Toggle camera iso and over shoulder
            // Move to proper target position with easing
            if (isIsometricView)
            {
                Debug.Log("Moving to isoCamPos");
                camera.transform.DOMove(isometricCamPosition.transform.position,
                    switchViewDuration).SetEase(Ease.InOutBack);
            } else
            {
                Debug.Log("Moving to shoulderCamPos");
                camera.transform.DOMove(thirdPersonCamPosition.transform.position,
                    switchViewDuration).SetEase(Ease.InOutBack);
            }
        }
    }
}