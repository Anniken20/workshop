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
        if (enemy.animator != null) enemy.animator.SetBool("HighNoonDuel", true);
    }

    public override void ExitState()
    {
        nav.updateRotation = true;
        nav.updatePosition = true;
        if (enemy.animator != null) enemy.animator.SetBool("HighNoonDuel", false);
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
}
