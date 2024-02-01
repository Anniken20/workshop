using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    private Transform player; // Reference to the player's transform

    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started chasing");
        nav.isStopped = false;
        nav.SetDestination(player.position);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue chasing the player
        nav.SetDestination(player.position);
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped chasing");
        nav.isStopped = true;
    }
}