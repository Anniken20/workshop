using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public GameObject shadow; // Assign your shadow GameObject in the inspector
    public Transform player; // Assign your player GameObject in the inspector
    public float growRate = 0.1f; // How quickly the shadow grows
    private bool playerInZone = false;
    private Vector3 lastPosition; // To track if the player has moved
    private float shadowSize;
    public float initialShadowSize = 0.1f; // Initial size of the shadow

    private void Start()
    {
        shadowSize = initialShadowSize;
        shadow.transform.localScale = Vector3.one * initialShadowSize;
        shadow.SetActive(false);
        lastPosition = player.position; // Initialize lastPosition
    }

    void Update()
    {
        if (playerInZone)
        {
            // Check if player has moved
            if (player.position != lastPosition)
            {
                // Player has moved, reset shadow
                ResetShadow();
                lastPosition = player.position; // Update lastPosition to the new position
            }
            else
            {
                // Player is still, grow shadow
                GrowShadow();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerInZone = true;
            lastPosition = player.position; // Reset lastPosition to ensure movement is detected afresh
            shadow.SetActive(true); // Make sure the shadow is visible
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerInZone = false;
            ResetShadow(); // Ensure shadow resets when player leaves the zone
            shadow.SetActive(false);
        }
    }

    void GrowShadow()
    {
        // Only grow shadow if it's active (visible)
        if (shadow.transform.localScale == Vector3.zero)
        {
        shadow.transform.localScale = new Vector3(initialShadowSize, initialShadowSize, initialShadowSize);
        }

        shadowSize += growRate * Time.deltaTime;
        shadow.transform.localScale = new Vector3(shadowSize, shadowSize, shadowSize);
        PositionShadowUnderPlayer();
    }

    void ResetShadow()
    {
        shadowSize = initialShadowSize;
        shadow.transform.localScale = Vector3.zero;
    }

    void PositionShadowUnderPlayer()
    {
        shadow.transform.position = new Vector3(player.position.x, shadow.transform.position.y, player.position.z);
    }
}