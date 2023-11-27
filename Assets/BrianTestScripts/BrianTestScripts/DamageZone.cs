using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
public int damageAmount = 10; // Amount of damage inflicted

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log("Player entered damage zone and took " + damageAmount + " damage.");
        }
    }
}
