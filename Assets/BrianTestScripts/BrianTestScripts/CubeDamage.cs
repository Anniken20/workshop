using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDamage : MonoBehaviour
{
     public int damageAmount = 10; // The amount of damage the cube will deal to the player

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Attempt to get the player's health or damage script
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // If the player has the health script component, deal damage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
    }
}
