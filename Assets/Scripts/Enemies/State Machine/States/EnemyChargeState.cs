using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class EnemyChargeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public float chargeSpeed = 5.0f; // Speed at which the enemy charges

    public EnemyChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
         
    }

    public override void EnterState()
    {
        base.EnterState();
        player = ThirdPersonController.Main.transform;
        nav.speed = chargeSpeed;
        nav.SetDestination(player.position);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Continue charging toward the player
        Debug.Log("charging towards " + player.position);
        nav.SetDestination(player.position);
    }

    public override void ExitState()
    {
        base.ExitState();
        nav.speed = enemy.defaultMovementSpeed; // Reset the speed to the default value
    }
}