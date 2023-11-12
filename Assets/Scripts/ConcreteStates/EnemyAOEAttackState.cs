using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAOEAttackState : EnemyState
{
    public GameObject aoeAttackPrefab; // The AOE attack effect prefab
    public Transform aoeAttackPoint; // The position where the AOE attack is centered
    public float aoeRadius = 5.0f; // The radius of the AOE effect
    public float attackCooldown = 5.0f; // Cooldown between AOE attacks
    private float nextAttackTime; // Time for the next AOE attack

    public EnemyAOEAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started AOE attack");
        nextAttackTime = Time.time;
        PerformAOEAttack();
    }

    public override void FrameUpdate()
    {
        if (CanPerformAOEAttack())
        {
            PerformAOEAttack();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped AOE attack");
    }

    private bool CanPerformAOEAttack()
    {
        return Time.time >= nextAttackTime;
    }

    private void PerformAOEAttack()
    {
        // Create the AOE attack effect
        GameObject aoeAttackEffect = Instantiate(aoeAttackPrefab, aoeAttackPoint.position, Quaternion.identity);

        // Apply the AOE attack within the specified radius
        Collider[] colliders = Physics.OverlapSphere(aoeAttackPoint.position, aoeRadius);

        foreach (Collider collider in colliders)
        {
            // Check if the collider belongs to player
            if (collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10); // Adjust the damage as needed
                }
            }
        }

        // Set the cooldown for the next AOE attack
        nextAttackTime = Time.time + attackCooldown;
    }

    public void SetupAttackData(GameObject aoeAttackPrefab, Transform aoeAttackPoint, float aoeRadius, float attackCooldown)
    {
        this.aoeAttackPrefab = aoeAttackPrefab;
        this.aoeAttackPoint = aoeAttackPoint;
        this.aoeRadius = aoeRadius;
        this.attackCooldown = attackCooldown;
    }

    //testing out scriptable objects. didnt quite work work game object and transform references -------------
    /*
    public void SetupAttackData(AOEAttackData data)
    {
        aoeAttackPrefab = data.aoeAttackPrefab;
        aoeAttackPoint = data.aoeAttackPoint;
        aoeRadius = data.aoeRadius;
        attackCooldown = data.attackCooldown;
    }
    */
}