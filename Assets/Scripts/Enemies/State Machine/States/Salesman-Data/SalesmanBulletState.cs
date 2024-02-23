using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesmanBulletState : EnemyState
{
    private SalesmanData salesData;
    public Transform targetLocation;
    private bool movingToPosition;
    private float distanceFromTarget;
    private Transform player;
    private bool atPlayer;

    public SalesmanBulletState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}
        public override void EnterState()
    {
        base.EnterState();
        salesData = (SalesmanData)enemy.FindData("SalesmanData");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav.speed = salesData.moveSpeed;
        nav.acceleration = salesData.acceleration;
        StopAllCoroutines();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Debug.Log(atPlayer);
        if(distanceFromTarget <= salesData.maxFollowDistance)
        {
            HeadToTarget(targetLocation, atPlayer);
        }
        if(movingToPosition)
        {
            distanceFromTarget = Vector3.Distance(this.gameObject.transform.position, nav.destination);
            //Debug.Log("Distance: " + distanceFromTarget +"Stopping Distance: " +salesData.stopFollowingDistance);

            if(distanceFromTarget <= salesData.stopFollowingDistance)
            {
                //Debug.Log("Reached Stopping Distance");
                if (atPlayer == false)
                {
                    StartCoroutine(WaitAtPoint());
                }
            }
        }
    }
    public override void ExitState()
    {
        base.ExitState();
    }
    public void HeadToTarget(Transform target, bool isPlayer)
    {
        if(target != null)
        {
            targetLocation = target;
            atPlayer = isPlayer;
            nav.SetDestination(target.position);
            movingToPosition = true;
        }
    }
    private IEnumerator WaitAtPoint()
    {
        //Debug.Log("Waiting At Point");
        nav.SetDestination(this.transform.position);
        movingToPosition = false;
        yield return new WaitForSeconds(salesData.locationWaitTime);
        if (atPlayer == false)
        {
            ReturnToPlayer();
        }
        else
        {
            nav.SetDestination(this.transform.position);
        }
    }
    private void ReturnToPlayer()
    {
        nav.SetDestination(player.position);
        //targetLocation = player;
        atPlayer = true;

        

    }


}

