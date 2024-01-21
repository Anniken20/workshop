using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 20; // Adjust the amount of health restored per pickup
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player object
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Increase player health and clamp it to the maximum health
                playerHealth.currentHealth += healthAmount;
                playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0, playerHealth.maxHealth);

                // Update health UI
                playerHealth.UpdateHealthUI();

                Debug.Log("Player health increased by " + healthAmount + ". Current health: " + playerHealth.currentHealth);

                // Disable or destroy the health pickup object
                gameObject.SetActive(false); // Alternatively, you can use Destroy(gameObject);
            }
        }
    }
}

