using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRisingController : MonoBehaviour
{
    public GameObject risingMarker; // Assign your rising marker GameObject in the inspector
    public GameObject risingCubePrefab; // Assign a prefab of the cube that will rise
    public Transform player; // Assign your player GameObject in the inspector
    public float growRate = 0.1f; // How quickly the rising marker grows
    private bool playerInZone = false;
    private Vector3 lastPosition; // To track if the player has moved
    private float markerSize;
    public float initialMarkerSize = 0.1f; // Initial size of the rising marker
    public float maxMarkerSize = 2f; // Maximum size of the rising marker before triggering the cube rise
    private float stillTime = 0f; // Time the player has been still
    public float maxStillTime = 5f; // Maximum time player can be still before triggering the cube rise

    private void Start()
    {
        markerSize = initialMarkerSize;
        risingMarker.transform.localScale = Vector3.zero; // Start with the marker "invisible"
        lastPosition = player.position;
    }

    void Update()
    {
        if (playerInZone)
        {
            if (player.position != lastPosition)
            {
                ResetMarkerAndTimer();
                lastPosition = player.position;
            }
            else
            {
                GrowMarker();
                stillTime += Time.deltaTime;

                if (risingMarker.transform.localScale.x >= maxMarkerSize || stillTime >= maxStillTime)
                {
                    TriggerRisingCube();
                    ResetMarkerAndTimer();
                }
            }
        }
    }

    void TriggerRisingCube()
    {
        // Instantiate the cube at player's position, emerging from below
        GameObject risingCube = Instantiate(risingCubePrefab, player.position + Vector3.down * 1, Quaternion.identity);
        risingCube.transform.Translate(Vector3.up * 1, Space.World); // Adjust starting point if needed
    }

    void ResetMarkerAndTimer()
    {
        ResetMarker();
        stillTime = 0; // Reset the timer
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerInZone = true;
            lastPosition = player.position; // Reset lastPosition to ensure movement is detected afresh
            risingMarker.SetActive(true); // Make sure the rising marker is visible
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerInZone = false;
            ResetMarker(); // Ensure marker resets when player leaves the zone
            risingMarker.SetActive(false);
        }
    }

    void GrowMarker()
    {
        // Only grow marker if it's active (visible)
        if (risingMarker.transform.localScale == Vector3.zero)
        {
            risingMarker.transform.localScale = new Vector3(initialMarkerSize, initialMarkerSize, initialMarkerSize);
        }

        markerSize += growRate * Time.deltaTime;
        risingMarker.transform.localScale = new Vector3(markerSize, markerSize, markerSize);
        PositionMarkerUnderPlayer();
    }

    void ResetMarker()
    {
        markerSize = initialMarkerSize;
        risingMarker.transform.localScale = Vector3.zero;
    }

    void PositionMarkerUnderPlayer()
    {
        risingMarker.transform.position = new Vector3(player.position.x, risingMarker.transform.position.y, player.position.z);
    }
}
