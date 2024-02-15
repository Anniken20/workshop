using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAOEAttackState : EnemyState
{
    private float nextAttackTime; // Time for the next AOE attack
    public AOEAttackData aoeAttackData;

    public EnemyAOEAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started AOE attack");

        //arbitrary 1 second buffer
        nextAttackTime = Time.time + 1f;
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
    }

    private bool CanPerformAOEAttack()
    {
        return Time.time >= nextAttackTime;
    }

    private void PerformAOEAttack()
    {
        // Create the AOE attack effect
        GameObject aoeAttackEffect = Instantiate(enemy.aoeAttackPrefab, enemy.aoeAttackPoint.position, Quaternion.identity);
        Destroy(aoeAttackEffect, 2f);

        /*
        // Apply the AOE attack within the specified radius
        Collider[] colliders = Physics.OverlapSphere(enemy.aoeAttackPoint.position, enemy.aoeRadius);

        foreach (Collider collider in colliders)
        {
            // Check if the collider belongs to player
            if (collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10); // Adjust the damage as needed
                    Debug.Log("DEALING AOE DAMAGE");
                }
            }
        }
        */

        // Set the cooldown for the next AOE attack
        nextAttackTime = Time.time + enemy.attackCooldown;
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