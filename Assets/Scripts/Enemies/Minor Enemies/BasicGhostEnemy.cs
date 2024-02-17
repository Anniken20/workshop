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

    private GameObject player; // Who ghost attacks and deals damage to
    private GameObject target; //where ghsot looks at and matches Y value with
    public int currentHealth; // Ghost health
    private bool canAttack = true; // Whether ghost can attack or not

    public AggroScript aggroScript; // ref to aggro script 
    public float lookAtOffset = 1.4f;

    public void OnShot(BulletController bullet)
    {
        TakeDamage((int)bullet.currDmg);
    }

    void Awake()
    {
        aggroScript = GetComponentInChildren<AggroScript>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    void Update()
    {

        if (aggroScript.target == null) // If the player is not found, do nothing
            return;

        // Calculate direction towards the player's position
        //set the Y to the shootpoint's Y, so it doesn't look at Val's feet, also negates need for float height by doing it automatically, and always being at shoot height
        Vector3 targetPosition = new Vector3(aggroScript.target.transform.position.x, aggroScript.target.transform.position.y + lookAtOffset, aggroScript.target.transform.position.z);
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        // Move towards the player's absolute position
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        // Ethan - look at target position
        this.gameObject.transform.LookAt(targetPosition);

        // Check if the player is within attack range and the enemy can attack
        //Since it's a trigger now, just set it to ontriggerstay so it isnt checking every frame, kept the code in case haunted objs need it later

        /*
         * if (Vector3.Distance(transform.position, player.transform.position) < attackRange && canAttack)
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
         */

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

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canAttack)
            {
                // Attack the player
                PlayerHealth playerHealth = aggroScript.target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    canAttack = false;
                    Invoke("ResetAttackCooldown", attackCooldown);
                }
            }
        }

    }
}
