using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLaunchAttackState : EnemyState
{
    private bool hasLaunched; // Flag to track if the enemy has performed a launch attack

    public EnemyLaunchAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started launch attack");
        hasLaunched = false;
        LaunchAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped launch attack");
    }

    private void LaunchAttack()
    {
        if (!hasLaunched)
        {
            // Create and launch the projectile
            GameObject projectile = Instantiate(enemy.launchProjectilePrefab, enemy.launchPoint.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Apply force to the projectile to launch it
            projectileRigidbody.AddForce(enemy.launchPoint.forward * enemy.launchForce, ForceMode.Impulse);

            hasLaunched = true;
        }
    }
}