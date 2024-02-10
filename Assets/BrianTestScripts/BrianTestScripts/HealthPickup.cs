using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public int healthAmount = 20; // Adjust the amount of health restored per pickup

    protected override void PickupAction()
    {
        // Attempt to get the PlayerHealth component from the player object
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>(); // Assuming the player is the only one with a PlayerHealth component

        if (playerHealth != null)
        {
            // Increase player health and ensure it does not exceed maximum health
            playerHealth.currentHealth += healthAmount;
            playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0, playerHealth.maxHealth);

            // Update the health UI to reflect the new health value
            playerHealth.UpdateHealthUI();

            Debug.Log("Player health increased by " + healthAmount + ". Current health: " + playerHealth.currentHealth);
        }

        base.PickupAction(); // Call the base method if there's any functionality there; it's optional since it's empty by default
    }
}