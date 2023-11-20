using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{   
    private Vector3 initialPosition;
    private Vector3 respawnPosition;

    public DeathScreenManager deathScreenManager;

    private void Start()
    {
        // Save the initial position as the first spawn point
        initialPosition = transform.position;
        respawnPosition = initialPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null)
        {
            // Update the respawn position when colliding with a checkpoint
            respawnPosition = checkpoint.transform.position;
            Debug.Log("Checkpoint reached! Position: " + respawnPosition); // Log checkpoint position
        }
        if (other.CompareTag("DeathZone"))
        {
            Die();
        }
    }

    public void Die()
    {
        deathScreenManager.ShowDeathScreen();
    }

    public void RestartFromCheckpoint()
    {
        transform.position = respawnPosition; // Move the player to the checkpoint position
        deathScreenManager.HideDeathScreen();
    }
}
