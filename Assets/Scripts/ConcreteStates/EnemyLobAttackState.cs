/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLobAttackState : EnemyState
{
    public GameObject lobProjectilePrefab; // The projectile to be lobbed
    public Transform lobTarget; // The target where the lobbed projectile will land
    public float lobSpeed = 10.0f; // Speed of the lobbed projectile
    private bool hasAttacked; // Flag to track if the enemy has performed a lob attack

    public EnemyLobAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started lob attack");
        hasAttacked = false;
        StartCoroutine(LobAttack());
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

        if (!hasAttacked)
        {
            // Create and lob the projectile
            GameObject projectile = Instantiate(lobProjectilePrefab, transform.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Calculate the direction to the lob target
            Vector3 direction = (lobTarget.position - transform.position).normalized;

            // Apply force to the projectile to make it land at the lob target
            projectileRigidbody.velocity = direction * lobSpeed;

            hasAttacked = true;
        }
    }
}
*/
