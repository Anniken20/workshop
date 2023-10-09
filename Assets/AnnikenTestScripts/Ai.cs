using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float shootingRange = 10.0f;
    public float shootingCooldown = 2.0f;
    public int maxHealth = 100;
    

    private int currentHealth;
    private float lastShootTime;

    private void Start()
    {
        currentHealth = maxHealth;
        lastShootTime = Time.time;
    }

    private void Update()
    {
        // Check if in shooting range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            // Rotate towards the player
            Vector3 direction = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if  it can shoot
            if (Time.time - lastShootTime >= shootingCooldown)
            {
                Shoot();
                lastShootTime = Time.time;
            }
        }
        else
        {
            // Move towards the player
            Vector3 moveDirection = (player.position - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void Shoot()
    {
        //Shoot thingy here we need the mechanic itself
        Debug.Log("Shooting at the player!");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implement death anim/effect? 
        Debug.Log("Boss died!");
        Destroy(gameObject);
    }
}
