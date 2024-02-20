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
      private Animator playerAnimator;

    void Start()
    {
        cage.SetActive(true);
        if (player != null)
        {
            playerController = player.GetComponent<ThirdPersonController>();
            playerAnimator = player.GetComponent<Animator>();
        }
    }

    // Public method to start the trap, callable by UnityEvent
    public void ActivateTrap()
    {
        StartCoroutine(TrapPlayer());
        playerAnimator.SetBool("IsGrounded", false);
    }

    IEnumerator TrapPlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        cage.SetActive(true);
        cage.transform.position = player.transform.position;

        yield return new WaitForSeconds(trapDuration);

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        cage.SetActive(false);
    }
}