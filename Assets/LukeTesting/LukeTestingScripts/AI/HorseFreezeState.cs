using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HorseFreezeState : EnemyState
{
    public HorseFreezeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        nav.isStopped = true;
        nav.SetDestination(this.transform.position);
    }

    public override void ExitState()
    {
        nav.isStopped = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
