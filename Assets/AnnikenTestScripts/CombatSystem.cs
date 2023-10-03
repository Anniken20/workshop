using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public CameraController cameraController; // Reference to the camera controller.

    // Start combat and switch to over-the-shoulder view.
    public void StartCombat()
    {
        cameraController.SwitchToOverShoulderView();
        // Add combat initialization logic here.
    }

    // End combat and switch back to isometric view.
    public void EndCombat()
    {
        cameraController.SwitchToIsometricView();
        // Add combat ending logic here.
    }
}