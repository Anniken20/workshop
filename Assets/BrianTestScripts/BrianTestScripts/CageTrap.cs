using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CageTrap : MonoBehaviour
{
     public GameObject player; 
    public GameObject cage; 
    public float trapDuration = 5.0f; // Time in seconds the player is trapped

    private ThirdPersonController playerController;

    void Start()
    {
        // Optionally, start with the cage disabled
        cage.SetActive(false);

        // Get the ThirdPersonController component from the player
        if (player != null)
        {
            playerController = player.GetComponent<ThirdPersonController>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(TrapPlayer());
        }
    }

    IEnumerator TrapPlayer()
    {
        // Disable player movement
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Activate the cage
        cage.SetActive(true);
        // Position the cage around the player
        cage.transform.position = player.transform.position;

        yield return new WaitForSeconds(trapDuration);

        // Re-enable player movement
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Deactivate the cage
        cage.SetActive(false);
    }
}
