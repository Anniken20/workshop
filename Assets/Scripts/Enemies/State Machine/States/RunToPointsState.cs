using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToPointsState : EnemyState
{
    public RunToPointsData runToPointsData;
    private Vector3 targetPoint;

    public RunToPointsState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        runToPointsData = (RunToPointsData)enemy.FindData("RunToPointsData");
        PickNextPoint();
        enemy.animator.SetBool("Running", true);
        Debug.Log("entered run to point state");
    }

    public override void FrameUpdate()
    {
        Debug.Log("target point: " + targetPoint);
        if (ReachedDestination()) {
            if (runToPointsData.cryAfterReachingDestination)
            {
                ((ICryable)enemy).StartCrying();
            }
        }
    }

    public override void ExitState()
    {
        enemy.animator.SetBool("Running", false);
        nav.SetDestination(transform.position);
        base.ExitState();
    }

    private void PickNextPoint()
    {
        targetPoint = runToPointsData.points[Random.Range(0, runToPointsData.points.Length - 1)];
        nav.SetDestination(targetPoint);
    }

    private bool ReachedDestination()
    {
        if(Vector3.Distance(transform.position, targetPoint) < runToPointsData.distanceTolerance)
            return true;
        else return false;
    }
}
