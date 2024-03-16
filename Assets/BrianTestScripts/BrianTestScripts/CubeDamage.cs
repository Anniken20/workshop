using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDamage : MonoBehaviour
{
     public int damageAmount = 10; 
     public float additionalGravity = -100000f; 

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * additionalGravity, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // If the player has the health script component, deal damage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
}
