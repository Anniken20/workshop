using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToPointsState : EnemyState
{
    public RunToPointsData runToPointsData;
    private Vector3 targetPoint;
    private Coroutine waitRoutine;

    public RunToPointsState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        nav.speed = enemy.defaultMovementSpeed;
        runToPointsData = (RunToPointsData)enemy.FindData("RunToPointsData");
        PickNextPoint();
        enemy.animator.SetBool("Idle", false);
        enemy.animator.SetBool("Crying", false);
        enemy.animator.SetBool("Running", true);
    }

    public override void FrameUpdate()
    {
        if (ReachedDestination()) {
            waitRoutine = StartCoroutine(WaitRoutine());
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
        if(waitRoutine != null) StopCoroutine(waitRoutine);
        targetPoint = runToPointsData.points[Random.Range(0, runToPointsData.points.Length)];
        enemy.animator.SetBool("Idle", false);
        enemy.animator.SetBool("Running", false);
        enemy.animator.SetBool("Crying", true);
        //loop til u find a new point
        while (ReachedDestination())
        {
            targetPoint = runToPointsData.points[Random.Range(0, runToPointsData.points.Length)];
        }

        nav.SetDestination(targetPoint);
        enemy.animator.SetBool("Idle", false);
        enemy.animator.SetBool("Crying", false);
        enemy.animator.SetBool("Running", true);
    }

    private bool ReachedDestination()
    {
        if(Vector3.Distance(transform.position, targetPoint) <= runToPointsData.distanceTolerance)
            return true;
        else return false;
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(runToPointsData.maxTimeAtPoint);
        PickNextPoint();
    }
}
