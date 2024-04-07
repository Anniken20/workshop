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

    private Coroutine trapRoutine;

    public AudioSource audioSource;
    public AudioClip clang;

    private void Start()
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
        trapRoutine = StartCoroutine(TrapPlayer());
    }

    public void DeactivateTrap()
    {
        if(trapRoutine != null) StopCoroutine(trapRoutine);
        FreePlayer();
    }

    private IEnumerator TrapPlayer()
    {
        if (playerController != null)
        {
            playerController.LockInPlace();
        }
        cage.SetActive(true);
        cage.transform.position = player.transform.position;
        audioSource.PlayOneShot(clang);

        yield return new WaitForSeconds(trapDuration);

        FreePlayer();
    }

    private void FreePlayer()
    {
        if (playerController != null)
        {
            playerController.FreeFromLockInPlace();
        }

        cage.SetActive(false);
    }
}