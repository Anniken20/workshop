using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorseChargeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public float maxChargeDist;
    private Transform startPos;
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges

    public HorseChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Started horse charging");
        nav.speed = chargeSpeed;
        nav.SetDestination(player.position);
        startPos = this.transform;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue charging toward the player
        //nav.SetDestination(player.position);
        float distance = Vector3.Distance(startPos.position, transform.position);
        if(distance >= maxChargeDist){
            ExitState();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Stopped charging");
        nav.speed = enemy.DefaultMovementSpeed; // Reset the speed to the default value
    }
}