using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GhostEnemy : MonoBehaviour, IShootable
{
    public float movementSpeed = 2f; // Speed at which the ghost moves towards the player
    public float attackRange = 1.5f; // Range within which the ghost attacks the player
    public int damage = 1; // Damage inflicted to the player when in attack range
    public int maxHealth = 50; // Maximum health of the ghost
    public float attackCooldown = 2f; // Cooldown time between attacks

    private GameObject player; // Who ghost attacks and deals damage to
    private GameObject target; // Where ghost looks at and matches Y value with
    public int currentHealth; // Ghost health
    private bool canAttack = true; // Whether ghost can attack or not

    public AggroScript aggroScript; // Reference to the aggro script 
    public float lookAtOffset = 1.4f;
    public AudioClip[] deathSounds;
    private AudioSource audioSource;

    private VisualEffect _visualEffectController;

    public float minDistanceBetweenEnemies = 2f; // Minimum distance between enemies

    public void OnShot(BulletController bullet)
    {
        TakeDamage((int)bullet.currDmg);
    }

    void Awake()
    {
        aggroScript = GetComponentInChildren<AggroScript>();
        _visualEffectController = GetComponentInChildren<VisualEffect>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (aggroScript.target == null) // If the player is not found, do nothing
            return;

        Vector3 targetPosition = new Vector3(aggroScript.target.transform.position.x, aggroScript.target.transform.position.y + lookAtOffset, aggroScript.target.transform.position.z);
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        // Move towards the player's position
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        // Look at the target position
        transform.LookAt(targetPosition);

        // Check if the player is within attack range and the enemy can attack
        if (Vector3.Distance(transform.position, targetPosition) < attackRange && canAttack)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        PlayerHealth playerHealth = aggroScript.target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            canAttack = false;
            Invoke("ResetAttackCooldown", attackCooldown);
        }
    }

    void ResetAttackCooldown()
    {
        canAttack = true;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            AvoidCollision(other.transform.position);
        }
    }

    void AvoidCollision(Vector3 otherPosition)
    {
        float distanceToEnemy = Vector3.Distance(transform.position, otherPosition);

        // If the distance is less than the minimum distance, move away from the other enemy
        if (distanceToEnemy < minDistanceBetweenEnemies)
        {
            Vector3 moveDirection = transform.position - otherPosition;
            transform.Translate(moveDirection.normalized * (minDistanceBetweenEnemies - distanceToEnemy) * Time.deltaTime, Space.World);
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
        StartCoroutine(DoEffectDeath(1.2f));
    }

    IEnumerator DoEffectDeath(float deathTime)
    {
        // Check if there are death sounds available
        if (deathSounds.Length > 0)
        {
            // Choose a random death sound
            AudioClip randomDeathSound = deathSounds[Random.Range(0, deathSounds.Length)];

            // Play the chosen death sound
            audioSource.PlayOneShot(randomDeathSound);
        }

        float currentTime = 0;
        while (currentTime < deathTime)
        {
            _visualEffectController.SetFloat("TimeDissolve", currentTime / deathTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
