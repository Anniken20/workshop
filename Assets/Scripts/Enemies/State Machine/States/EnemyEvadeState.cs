using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEvadeState : EnemyState
{
    private Transform player; // Reference to the player's transform
    public EvadeData evadeData;
    public EnemyEvadeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    public override void EnterState()
    {
        base.EnterState();
        evadeData = (EvadeData)enemy.FindData("evadeData");
        Debug.Log("Started evading");
        nav.speed = evadeData.evadeSpeed;
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
        nav.speed = enemy.defaultMovementSpeed; // Reset the speed to the default value
    }

    private void CalculateEvadePosition()
    {
        // Calculate a position away from the player by the evadeDistance
        Vector3 evadePosition = transform.position + (transform.position - player.position).normalized * evadeData.evadeDistance;
        nav.SetDestination(evadePosition);
    }
}
