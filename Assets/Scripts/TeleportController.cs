using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using Cinemachine;

public class TeleportController : MonoBehaviour
{
    [Header("For 2-ways")]
    public TeleportController otherLink;
    public bool active;
    [Header("General setup")]
    public GameObject toPoint;
    public float walkDistance = 1.0f; // Adjust this distance as needed
    public float forwardDistance = 2.0f; // Edit this value in the Inspector
    public Axis forwardAxis = Axis.Z; // Choose the forward axis in the Inspector
    public float walkDuration = 1.0f; // Adjust this duration as needed
    public float moveAfterTeleportDistance = 3.0f; // Distance to move after teleporting
    public float moveAfterTeleportDuration = 2.0f; // Duration to move after teleporting
    public float initialDelay = 0.1f; // Adjust this delay to reduce the stutter

    [Header("Fade")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    private float activateLink2Time;

    public enum Axis
    {
        X,
        Z
    }

    private CharacterController characterController;
    private bool isTransitioning = false;
    private Vector3 teleportDestination;
    private Vector3 intermediatePosition;

    [Header("Invincibility")]
    public float invincibilityDuration = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        if (other.gameObject.CompareTag("Player") && !isTransitioning)
        {
            characterController = other.gameObject.GetComponent<CharacterController>();
            StartCoroutine(TransitionThroughTeleporter(other.gameObject));
        }
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }

     private IEnumerator FadeFromBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
    }

    IEnumerator TransitionThroughTeleporter(GameObject player)
    {
        isTransitioning = true;

        // Disable character control
        characterController.enabled = false;
        ThirdPersonController.Main._inCinematic = true;

        StartCoroutine(FadeToBlack());

        // Calculate the intermediate position at the center of the teleporter on X/Z axis
        Vector3 teleporterPosition = transform.position;
        intermediatePosition = new Vector3(teleporterPosition.x, player.transform.position.y, teleporterPosition.z);

        // Wait for a short initial delay
        yield return new WaitForSeconds(initialDelay);

        //play walk animation

        // Calculate the number of steps based on the walk duration
        int numSteps = Mathf.FloorToInt(walkDuration / Time.fixedDeltaTime);
        float stepDistance = Vector3.Distance(player.transform.position, intermediatePosition) / numSteps;

        // Move the player step by step to the center of the teleporter on X/Z axis
        for (int step = 0; step < numSteps; step++)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, intermediatePosition, stepDistance);
            yield return new WaitForFixedUpdate();
        }

        // Calculate the destination position in front of the teleporter
        Vector3 forwardDestination = intermediatePosition;

        // Adjust the forward axis based on the enum
        switch (forwardAxis)
        {
            case Axis.X:
                forwardDestination += transform.right * forwardDistance;
                break;
            case Axis.Z:
                forwardDestination += transform.forward * forwardDistance;
                break;
        }

        // Calculate the number of steps based on the walk duration
        numSteps = Mathf.FloorToInt(walkDuration / Time.fixedDeltaTime);
        stepDistance = Vector3.Distance(intermediatePosition, forwardDestination) / numSteps;

        // Move the player step by step forward
        for (int step = 0; step < numSteps; step++)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, forwardDestination, stepDistance);
            yield return new WaitForFixedUpdate();
        }

        // Store the teleport destination
        teleportDestination = toPoint.transform.position;

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
        Camera.main.GetComponent<CameraController>().RecomposeCamera();

        // Move the player for an additional distance after teleporting
        Vector3 moveAfterTeleportDestination = player.transform.position + player.transform.forward * moveAfterTeleportDistance;
        startTime = Time.time;
        elapsedTime = 0f;
        while (elapsedTime < moveAfterTeleportDuration)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, moveAfterTeleportDestination, stepDistance);
            elapsedTime = Time.time - startTime;
            yield return new WaitForFixedUpdate();
        }

        // Wait for a very short moment to avoid jitter
        yield return new WaitForSeconds(0.1f);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.SetInvincibility(invincibilityDuration);
        }

        // Re-enable character control
        characterController.enabled = true;
        ThirdPersonController.Main._inCinematic = false;

        //set walking animation back to false

        yield return StartCoroutine(FadeFromBlack());

        isTransitioning = false;

        yield return new WaitForSeconds(activateLink2Time);
        if (otherLink != null)
        {
            otherLink.active = true;
            active = false;
        }

        //arbitrary value but makes sure that the camera is back to the normal angle before it switches over
        yield return new WaitForSeconds(8f);
    }

    public void ForceActive(bool active2)
    {
        if(active2)
        {
            active = true;
        }
        if(!active2)
        {
            active = false;
        }
    }
}