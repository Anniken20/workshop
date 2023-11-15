using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHidingState : EnemyState
{
    private Transform player; // Reference to the player's transform

    public EnemyHidingState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started hiding");
        nav.speed = enemy.hidingSpeed;
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
        nav.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
    }

    private void CalculateHidingSpot()
    {
        // Calculate a position away from the player by the hidingDistance
        Vector3 hidingSpot = transform.position + (transform.position - player.position).normalized * enemy.hidingDistance;
        nav.SetDestination(hidingSpot);
    }
}