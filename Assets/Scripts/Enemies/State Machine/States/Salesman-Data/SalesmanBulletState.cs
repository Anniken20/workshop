using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SalesmanBulletState : EnemyState
{
    [HideInInspector] public SalesmanData salesData;
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
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        player = ThirdPersonController.Main.gameObject.transform;
        nav.speed = salesData.bulletmoveSpeed;
        nav.acceleration = salesData.bulletAcceleration;
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
            nav.SetDestination(targetLocation.position);
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
            //targetLocation = FindObjectOfType<GhostBulletController>();
            atPlayer = isPlayer;
            nav.SetDestination(target.position);
            if (enemy.animator != null) enemy.animator.SetBool("Running", true);
            movingToPosition = true;
        }
    }
    private IEnumerator WaitAtPoint()
    {
        //Debug.Log("Waiting At Point");
        if (enemy.animator != null) enemy.animator.SetBool("Idle", true);
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
        //nav.SetDestination(player.position);
        this.GetComponent<Salesman>().ChaseState();
        if (enemy.animator != null) enemy.animator.SetBool("Running", true);
        //targetLocation = player;
        atPlayer = true;

        

    }


}

