using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject walkToPoint;
    public GameObject waitPoint;
    public GameObject teleportPoint;
    private CharacterController characterController;
    private bool isTransitioning = false;
    public float walkSpeed = 2.0f;  // Adjust this as needed
    public float waitDuration = 2.0f;  // Adjust this as needed
    private Vector3 teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTransitioning)
        {
            Debug.Log("Player triggered the teleport");
            characterController = other.gameObject.GetComponent<CharacterController>();
            StartCoroutine(TransitionSequence(other.gameObject));
        }
    }

    IEnumerator TransitionSequence(GameObject player)
    {
        isTransitioning = true;

        characterController.enabled = false;

        // Walk to the designated walk point
        Debug.Log("Walking to the walk point");
        yield return WalkToPosition(player, walkToPoint.transform.position);

        // Wait at the wait point for the specified duration
        Debug.Log("Waiting at the wait point");
        yield return new WaitForSeconds(waitDuration);

        // Teleport to the final destination
        Debug.Log("Teleporting to the final destination");
        player.transform.position = teleportPoint.transform.position;

        // Wait for a very short moment to avoid jitter
        yield return new WaitForSeconds(0.1f);

        characterController.enabled = true;

        isTransitioning = false;
    }

    IEnumerator WalkToPosition(GameObject player, Vector3 targetPosition)
    {
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, walkSpeed * Time.deltaTime);
            yield return null;
        }
    }
}