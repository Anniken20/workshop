using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChargeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges

    public EnemyChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started charging");
        nav.speed = chargeSpeed;
        nav.SetDestination(player.position);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue charging toward the player
        nav.SetDestination(player.position);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped charging");
        nav.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
    }
}