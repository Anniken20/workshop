using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
   public GameObject toPoint;
    private CharacterController characterController;
    private bool isTransitioning = false;
    public float walkDistance = 1.0f;  // Adjust this distance as needed

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTransitioning)
        {
            characterController = other.gameObject.GetComponent<CharacterController>();
            StartCoroutine(TransitionThroughDoor(other.gameObject));
        }
    }

    IEnumerator TransitionThroughDoor(GameObject player)
    {
        isTransitioning = true;

        characterController.enabled = false;

        // Wait for a moment to simulate being frozen
        //Change this value as needed
        yield return new WaitForSeconds(1.0f);

        // Calculate the intermediate position just in front of the door
        Vector3 playerForward = player.transform.forward;
        Vector3 doorPosition = toPoint.transform.position;
        Vector3 intermediatePosition = doorPosition - playerForward * walkDistance;

        // Move the player to the intermediate position
        player.transform.position = intermediatePosition;

        // Teleport the player to the final destination
        player.transform.position = doorPosition;

        characterController.enabled = true;

        isTransitioning = false;
    }
}

