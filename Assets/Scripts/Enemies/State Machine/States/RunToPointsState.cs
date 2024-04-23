using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToPointsState : EnemyState
{
    public RunToPointsData runToPointsData;
    private Vector3 targetPoint;
    private Coroutine waitRoutine;
    private bool atTarget;

    public RunToPointsState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        nav.speed = enemy.defaultMovementSpeed;
        runToPointsData = (RunToPointsData)enemy.FindData("RunToPointsData");
        atTarget = false;
        PickNextPoint();
        enemy.animator.SetBool("Running", true);
    }

    public override void FrameUpdate()
    {
        if (!atTarget && ReachedDestination()) {
            atTarget = true;
            enemy.animator.SetBool("Running", false);
            waitRoutine = StartCoroutine(WaitRoutine());
            if (runToPointsData.cryAfterReachingDestination)
            {
                //Debug.Log("started crying");
                ((ICryable)enemy).StartCrying();
                enemy.animator.SetBool("Crying", true);
            } else
            {
                enemy.animator.SetBool("Idle", true);
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
        //loop til u find a new point
        while (ReachedDestination())
        {
            targetPoint = runToPointsData.points[Random.Range(0, runToPointsData.points.Length)];
        }
        if (runToPointsData.cryAfterReachingDestination)
        {
            //Debug.Log("stopped crying");
            ((ICryable)enemy).StopCrying();
            enemy.animator.SetBool("Crying", false);
            atTarget = false;
        }
        enemy.animator.SetBool("Running", true);
        nav.SetDestination(targetPoint);
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
