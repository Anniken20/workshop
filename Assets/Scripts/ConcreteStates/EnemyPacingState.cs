using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPacingState : EnemyState
{
    private float currentTimer;

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
        StartPace();
    }

    //start a random timer between 2 values
    private void StartPace()
    {
        currentTimer = Random.Range(((Boring)enemy).frequencyBounds.x, ((Boring)enemy).frequencyBounds.y);
    }

    public override void ExitState()
    {
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
            nav.SetDestination(enemy.gameObject.transform.position + (Random.insideUnitSphere * ((Boring)enemy).randomPointRadius));

            //set new timer to go to another point
            StartPace();
        }
    }

    //this is akin to a Unity FixedUpdate loop
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
