/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent; // Reference to the enemy's NavMeshAgent

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started chasing");
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue chasing the player
        navMeshAgent.SetDestination(player.position);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped chasing");
        navMeshAgent.isStopped = true;
    }
}
*/