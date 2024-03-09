using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class DuelState : EnemyState
{
    protected DuelState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        nav.updateRotation = false;
        nav.updatePosition = false;
        transform.LookAt(ThirdPersonController.Main.transform);
    }

    public override void ExitState()
    {
        nav.updateRotation = true;
        nav.updatePosition = true;
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
}
