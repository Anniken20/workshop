using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class EnemyHidingState : EnemyState
{
    private Transform player; // Reference to the player's transform
    private HidingData hidingData;

    public EnemyHidingState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        player = ThirdPersonController.Main.transform;
        hidingData = (HidingData)enemy.FindData("HidingData");
        //Debug.Log("Started hiding");
        nav.speed = hidingData.hidingSpeed;
        if (enemy.animator != null) enemy.animator.SetBool("Running", true);
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
        //Debug.Log("Stopped hiding");
        nav.speed = enemy.defaultMovementSpeed; // Reset the speed to the default value
    }

    private void CalculateHidingSpot()
    {
        // Calculate a position away from the player by the hidingDistance
        Vector3 hidingSpot = transform.position + (transform.position - player.position).normalized * hidingData.hidingDistance;
        nav.SetDestination(hidingSpot);
    }
}