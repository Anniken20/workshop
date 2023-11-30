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
        //Move the player to the checkpoint position

        //must temporarily turn off character controller to modify the transform
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = respawnPosition;
        cc.enabled = true;
    }

    public void RestartFromPosition(Vector3 pos)
    {
        transform.position = pos;
        deathScreenManager.HideDeathScreen();
    }
}
