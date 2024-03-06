using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
   public GameObject shadow; // Assign your shadow GameObject in the inspector
    public GameObject fallingCubePrefab; // Assign a prefab of the cube that will fall
    public Transform player; // Assign your player GameObject in the inspector
    public float growRate = 0.1f; // How quickly the shadow grows
    private bool playerInZone = false;
    private Vector3 lastPosition; // To track if the player has moved
    private float shadowSize;
    public float initialShadowSize = 0.1f; // Initial size of the shadow
    public float maxShadowSize = 2f; // Maximum size of the shadow before triggering the cube fall
    private float stillTime = 0f; // Time the player has been still
    public float maxStillTime = 5f; // Maximum time player can be still before triggering the cube fall

    private void Start()
    {
        shadowSize = initialShadowSize;
        shadow.transform.localScale = Vector3.zero; // Start with the shadow "invisible"
        lastPosition = player.position;
    }

    void Update()
    {
        if (playerInZone)
        {
            if (player.position != lastPosition)
            {
                ResetShadowAndTimer();
                lastPosition = player.position;
            }
            else
            {
                GrowShadow();
                stillTime += Time.deltaTime;

                if (shadow.transform.localScale.x >= maxShadowSize || stillTime >= maxStillTime)
                {
                    TriggerFallingCube();
                    ResetShadowAndTimer();
                }
            }
        }
    }

    void TriggerFallingCube()
    {
        // Instantiate the cube at player's position plus some height
        GameObject fallingCube = Instantiate(fallingCubePrefab, player.position + Vector3.up * 5, Quaternion.identity);
    }

    void ResetShadowAndTimer()
    {
        ResetShadow();
        stillTime = 0; // Reset the timer
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