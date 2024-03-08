using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SalesmanChasePlayerState : EnemyState
{
    private SalesmanData salesData;
    private Transform player;
    public SalesmanChasePlayerState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }
    public override void EnterState()
    {
        base.EnterState();
        salesData = (SalesmanData)enemy.FindData("SalesmanData");
        player = ThirdPersonController.Main.gameObject.transform;
        nav.speed = salesData.chasemoveSpeed;
        nav.acceleration = salesData.chaseAcceleration;
        nav.stoppingDistance = salesData.stopFollowingDistance;
        //nav.SetDestination(player.position);
    }
    public override void PhysicsUpdate()
    {
        var distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, player.position);
        //Debug.Log(distanceFromTarget);
        if (distanceFromTarget <= salesData.stopFollowingDistance)
        {
            nav.SetDestination(this.transform.position);
        }
        else
        {
            nav.SetDestination(player.position);
        }
    }
    public override void ExitState() { base.ExitState();}

}
