using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GhostEnemy : MonoBehaviour, IShootable
{
    public bool propGhost = false;
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

    private bool canMove = true; // Flag to control movement

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

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource> ();
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
        if (canMove)
            transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        // Ethan - look at target position
        this.gameObject.transform.LookAt(targetPosition);
        // Look at the target position
        transform.LookAt(targetPosition);

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

        // Check if the player is within attack range and the enemy can attack
        if (Vector3.Distance(transform.position, targetPosition) < attackRange && canAttack)
        {
            AttackPlayer();
        }

        // This creates a swarm like effect without being inside the player
        // I kinda liked this as once one is close, hits it'll be pushed back
        // Change the 1f on the bottom of this section to change how far they end up
        // Around the player (:
        //Avoid colliding with the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, movementSpeed * Time.deltaTime))
        {
            if (hit.collider.CompareTag("Player"))
            {
                float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);
                float minDistance = Mathf.Max(minDistanceBetweenEnemies, attackRange); // Ensure the minimum distance is at least the attack range
                if (distanceToPlayer < minDistance)
                {
                    // Adjust position to maintain minimum distance from the player
                    float pushBackDistance = minDistance - distanceToPlayer;
                    transform.position += direction * pushBackDistance;
                }
                else
                {
                    // Adjust position to not go inside the player
                    transform.position = hit.point - direction * 1f; // Adjust to how far away
                }
            }
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
        if (other.gameObject.CompareTag("Player"))
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
            /*if (canAttack)
            {
                // Attack the player
                PlayerHealth playerHealth = aggroScript.target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    canAttack = false;
                    Invoke("ResetAttackCooldown", attackCooldown);
                }
            }*/
            Die();
        }
    }

    void Die()
    {
        // Perform death-related action
        if (propGhost == true)
            StartCoroutine(DoEffectDeath(0f));
        else if (propGhost == false)
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
            _visualEffectController.SetFloat("TimeDissolve", currentTime / deathTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
