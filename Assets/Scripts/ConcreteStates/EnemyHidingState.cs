using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHidingState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent; // Reference to the enemy's NavMeshAgent
    public float hidingDistance = 10.0f; // Minimum distance at which the enemy considers hiding
    public float hidingSpeed = 5.0f; // Speed at which the enemy moves to a hiding spot

    public EnemyHidingState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started hiding");
        navMeshAgent.speed = hidingSpeed;
        CalculateHidingSpot();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue moving to the hiding spot
        CalculateHidingSpot();
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped hiding");
        navMeshAgent.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
    }

    private void CalculateHidingSpot()
    {
        // Calculate a position away from the player by the hidingDistance
        Vector3 hidingSpot = transform.position + (transform.position - player.position).normalized * hidingDistance;
        navMeshAgent.SetDestination(hidingSpot);
    }
}