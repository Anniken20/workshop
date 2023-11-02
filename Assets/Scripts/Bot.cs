using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
!Has bugs!
Sometimes unpredictable ones

Last edited by 11/02/23 Anniken creator of said AI
*/
public class Bot : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Melee Attack
    public GameObject projectile;
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // Melee Attack
    public float meleeRange;
    public int meleeDamage;
    public float meleeCooldown;
    bool canMelee = true;

    // Evasion
    public float evadeRange;
    public Transform hideObject;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();
    }
    private void SeekCover()
    {
        // Check if there's a hideObject defined
        if (hideObject != null)
        {
            // Calculate a position behind the hideObject
            Vector3 coverPosition = hideObject.position - (player.position - hideObject.position).normalized * 2f;

            // Move to the coverPosition
            agent.SetDestination(coverPosition);
        }
    }
    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Check if the player is in attack range
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            SeekCover();
            EvadePlayer();
        }
        else if (!alreadyAttacked)
        {
    
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Projectile attack
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

        if (!alreadyAttacked)
        {
            // Perform melee attack if player is in melee range
            if (Vector3.Distance(transform.position, player.position) <= meleeRange && canMelee)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
                canMelee = false;
                Invoke(nameof(ResetMeleeCooldown), meleeCooldown);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void EvadePlayer()
    {
        // Move away from the player in the opposite direction
        Vector3 evadeDirection = transform.position - player.position;
        Vector3 evadePoint = transform.position + evadeDirection.normalized * evadeRange;
        agent.SetDestination(evadePoint);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetMeleeCooldown()
    {
        canMelee = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
