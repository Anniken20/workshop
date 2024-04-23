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
        Debug.Log("entered cry state");
    }

    public override void ExitState()
    {
        if (enemy.animator != null) enemy.animator.SetBool("Crying", false);
        Debug.Log("exited cry state");
        base.ExitState();
    }
}
