/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttackState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public float attackRange = 2.0f; // Range at which the enemy can perform a melee attack
    public int attackDamage = 10; // Damage dealt by the melee attack
    public float attackCooldown = 1.0f; // Cooldown between melee attacks
    private float nextAttackTime; // Time for the next melee attack

    public EnemyMeleeAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started melee attack");
        nextAttackTime = Time.time;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (CanMeleeAttack())
        {
            MeleeAttack();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped melee attack");
    }

    private bool CanMeleeAttack()
    {
        return Time.time >= nextAttackTime && IsPlayerInRange();
    }

    private bool IsPlayerInRange()
    {
        // Check if the player is within the attack range
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    private void MeleeAttack()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        // Set the cooldown for the next melee attack
        nextAttackTime = Time.time + attackCooldown;
    }
}
*/