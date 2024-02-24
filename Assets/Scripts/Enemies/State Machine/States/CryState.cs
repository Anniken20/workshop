using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryState : EnemyState
{

    public CryState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        if (enemy.animator != null) enemy.animator.SetBool("Crying", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        if (enemy.animator != null) enemy.animator.SetBool("Crying", false);
    }
}
