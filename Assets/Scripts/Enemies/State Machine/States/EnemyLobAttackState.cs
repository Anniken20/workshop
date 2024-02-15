using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLobAttackState : EnemyState
{
    private bool hasAttacked; // Flag to track if the enemy has performed a lob attack
    //private float timeBetweenAttacks = 1f;
    //private float timer;
    private LobData lobData;

    public EnemyLobAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started lob attack");
        lobData = (LobData)enemy.FindData("LobData");
        //timer -= Time.deltaTime;
        //if(timer < 0f)
        //{
            StartCoroutine(LobAttack());
        //}
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped lob attack");
    }

    private IEnumerator LobAttack()
    {
        // Wait for a moment before performing the lob attack (optional)
        yield return new WaitForSeconds(1.0f);
        //timer = timeBetweenAttacks;

        if (!hasAttacked)
        {
            // Create and lob the projectile
            GameObject projectile = Instantiate(lobData.lobProjectilePrefab, enemy.firePoint.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            Destroy(projectile, 10f);

            // Calculate the direction to the lob target
            Vector3 direction = (enemy.firePoint.position - transform.position).normalized;

            // Apply force to the projectile to make it land at the lob target
            projectileRigidbody.velocity = direction * lobData.lobSpeed;

            hasAttacked = true;
        }
    }
}
