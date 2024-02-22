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
        cage.SetActive(true);
        if (player != null)
        {
            playerController = player.GetComponent<ThirdPersonController>();
        }
    }

    // Public method to start the trap, callable by UnityEvent
    public void ActivateTrap()
    {
        StartCoroutine(TrapPlayer());
    }

    IEnumerator TrapPlayer()
    {
        if (playerController != null)
        {
            playerController.LockInPlace();
        }
        cage.SetActive(true);
        cage.transform.position = player.transform.position;

        yield return new WaitForSeconds(trapDuration);

        if (playerController != null)
        {
            playerController.FreeFromLockInPlace();
        }

        cage.SetActive(false);
    }
}