using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttackState : EnemyState
{
    private Transform player; // Reference to the player's transform
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
        return Vector3.Distance(transform.position, player.position) <= enemy.attackRange;
    }

    private void MeleeAttack()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemy.attackDamage);
        }

        // Set the cooldown for the next melee attack
        nextAttackTime = Time.time + enemy.attackCooldown;
    }
}