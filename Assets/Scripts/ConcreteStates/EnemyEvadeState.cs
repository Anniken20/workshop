using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEvadeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent; // Reference to the enemy's NavMeshAgent
    public float evadeDistance = 5.0f; // Distance at which the enemy starts evading
    public float evadeSpeed = 5.0f; // Speed at which the enemy evades

    public EnemyEvadeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started evading");
        navMeshAgent.speed = evadeSpeed;
        CalculateEvadePosition();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue evading away from the player
        CalculateEvadePosition();
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped evading");
        navMeshAgent.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
    }

    private void CalculateEvadePosition()
    {
        // Calculate a position away from the player by the evadeDistance
        Vector3 evadePosition = transform.position + (transform.position - player.position).normalized * evadeDistance;
        navMeshAgent.SetDestination(evadePosition);
    }
}
