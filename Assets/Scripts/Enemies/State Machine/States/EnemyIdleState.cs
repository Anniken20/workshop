using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        if(nav.isOnNavMesh && nav.isActiveAndEnabled) nav.isStopped = true;
        if (enemy.animator != null) enemy.animator.SetBool("Idle", true);
    }

    public override void ExitState()
    {
        nav.isStopped = false;
        if (enemy.animator != null) enemy.animator.SetBool("Idle", false);
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
