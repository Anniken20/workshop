using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
   public GameObject toPoint;
    private CharacterController characterController;
    private bool isTransitioning = false;
    public float walkDistance = 1.0f;

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

        //Determines direction player is facing
        Vector3 playerForward = player.transform.forward;

        //Calculates destination position
        Vector3 destination = toPoint.transform.position;

        //Calculates offset position in front of midpoint
        Vector3 offset = playerForward * walkDistance;
        Vector3 midpoint = (player.transform.position + offset + destination) / 2f;

        float transitionDuration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            //Interpolates position to create smoother transition
            player.transform.position = Vector3.Lerp(player.transform.position, midpoint, elapsedTime / transitionDuration);

            //Rotates player to face same direction
            player.transform.forward = playerForward;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = destination;

        characterController.enabled = true;

        isTransitioning = false;
    }
}
