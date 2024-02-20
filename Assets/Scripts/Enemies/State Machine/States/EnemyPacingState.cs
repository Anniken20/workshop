using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPacingState : EnemyState
{
    private float currentTimer;
    private Animator animator;
    private PacingData pacingData;

    //constructor
    public EnemyPacingState(Enemy enemy, EnemyStateMachine enemyStateMachine): base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        pacingData = (PacingData)enemy.FindData("PacingData");
        StartPace();
        if(enemy.animator != null) enemy.animator.SetBool("Pacing", true);
    }

    //start a random timer between 2 values
    private void StartPace()
    {
        currentTimer = Random.Range(pacingData.frequencyBounds.x, pacingData.frequencyBounds.y);
    }

    public override void ExitState()
    {
        if (enemy.animator != null) enemy.animator.SetBool("Pacing", false);
        base.ExitState();
    }

    //this is akin to a Unity update loop
    public override void FrameUpdate()
    {
        //decrement our timer
        currentTimer -= Time.deltaTime;

        //time has elapsed
        if(currentTimer < 0)
        {
            //set new random point to go to
            nav.SetDestination(enemy.gameObject.transform.position + (Random.insideUnitSphere * pacingData.randomPointRadius));

            //set new timer to go to another point
            StartPace();
        }
        animator.SetBool("Moving", nav.remainingDistance > 0);

    }

    //this is akin to a Unity FixedUpdate loop
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
