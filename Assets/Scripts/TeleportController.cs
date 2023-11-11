using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject toPoint;
    private CharacterController characterController;
    private bool isTransitioning = false;
    public float walkDistance = 5.0f;  //Adjust this as needed
    public float walkDuration = 1.0f;  // Adjust this as needed
    private Vector3 teleportDestination;
    private Vector3 intermediatePosition;

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

        // Calculate the intermediate position just in front of the door
        Vector3 playerForward = player.transform.forward;
        Vector3 doorPosition = toPoint.transform.position;
        intermediatePosition = doorPosition - playerForward * walkDistance; 

        // Calculate the number of steps based on the walk duration
        int numSteps = Mathf.FloorToInt(walkDuration / Time.fixedDeltaTime);
        float stepDistance = Vector3.Distance(player.transform.position, intermediatePosition) / numSteps;

        // Move the player step by step
        for (int step = 0; step < numSteps; step++)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, intermediatePosition, stepDistance);
            yield return new WaitForFixedUpdate();
        }

        // Store the teleport destination
        teleportDestination = doorPosition;

        // Wait for the specified walk duration
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < walkDuration)
        {
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        // Teleport the player to the final destination
        player.transform.position = teleportDestination;

        // Wait for a very short moment to avoid jitter
        yield return new WaitForSeconds(0.1f);

        characterController.enabled = true;

        isTransitioning = false;
    }
}