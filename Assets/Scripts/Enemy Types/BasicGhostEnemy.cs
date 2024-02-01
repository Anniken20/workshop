using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : MonoBehaviour, IShootable
{
    public float movementSpeed = 2f; // Speed at which the ghost moves towards the player
    public float attackRange = 1.5f; // Range within which the ghost attacks the player
    public int damage = 1; // Damage inflicted to the player when in attack range
    public int maxHealth = 50; // Maximum health of the ghost
    public float attackCooldown = 2f; // Cooldown time between attacks
    public float floatingHeight = 1f; // Height above the player's position to float

    private GameObject player; // Who ghost attacks and deals damage to
    public int currentHealth; // Ghost health
    private bool canAttack = true; // Whether ghost can attack or not

    public void OnShot(BulletController bullet)
    {
        TakeDamage((int)bullet.currDmg);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player == null) // If the player is not found, do nothing
            return;

        // Calculate direction towards the player's position
        Vector3 targetPosition = player.transform.position + Vector3.up * floatingHeight;
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        // Move towards the player's absolute position
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        // Check if the player is within attack range and the enemy can attack
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange && canAttack)
        {
            // Attack the player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                canAttack = false;
                Invoke("ResetAttackCooldown", attackCooldown);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Perform death-related actions (e.g., play death animation, drop items, etc.)
        Destroy(gameObject);
    }

    void ResetAttackCooldown()
    {
        canAttack = true;
    }
}
