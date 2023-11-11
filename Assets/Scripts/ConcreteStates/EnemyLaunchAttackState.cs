/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLaunchAttackState : EnemyState
{
    public GameObject launchProjectilePrefab; // The projectile to be launched
    public Transform launchPoint; // The position where the projectiles are launched from
    public float launchForce = 10.0f; // The force with which the projectile is launched
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
            GameObject projectile = Instantiate(launchProjectilePrefab, launchPoint.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Apply force to the projectile to launch it
            projectileRigidbody.AddForce(launchPoint.forward * launchForce, ForceMode.Impulse);

            hasLaunched = true;
        }
    }
}
*/