using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HorseStunnedState : EnemyState
{
    [HideInInspector] public bool isStunned;
    public GameObject lassoTarget;
    public HorseStunnedState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        isStunned = true;
        Enemy h = GetComponentInParent<Enemy>();
        lassoTarget = h.lassoTarget;
        nav.isStopped = true;
        lassoTarget.GetComponent<BoxCollider>().enabled = true;
    }

    public override void PhysicsUpdate(){
        base.PhysicsUpdate();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void ExitState()
    {
        isStunned = false;
        nav.isStopped = false;
        lassoTarget.GetComponent<BoxCollider>().enabled = false;
    }

}
