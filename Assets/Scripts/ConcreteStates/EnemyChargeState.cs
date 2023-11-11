/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChargeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent; // Reference to the enemy's NavMeshAgent
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges

    public EnemyChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started charging");
        navMeshAgent.speed = chargeSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue charging toward the player
        navMeshAgent.SetDestination(player.position);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped charging");
        navMeshAgent.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
    }
}
*/