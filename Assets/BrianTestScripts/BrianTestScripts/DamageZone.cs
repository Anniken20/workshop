using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage inflicted
    public float damageInterval = 1.0f; // Time in seconds between each damage tick

    private Coroutine damageCoroutine;

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null && damageCoroutine == null)
        {
            // Start dealing damage over time
            damageCoroutine = StartCoroutine(ApplyDamageOverTime(playerHealth));
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null && damageCoroutine != null)
        {
            // Stop dealing damage as player exits the zone
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    IEnumerator ApplyDamageOverTime(PlayerHealth playerHealth)
    {
        while (true)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Player is in the damage zone and took " + damageAmount + " damage.");
            
            // Wait for specified interval before applying damage again
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
