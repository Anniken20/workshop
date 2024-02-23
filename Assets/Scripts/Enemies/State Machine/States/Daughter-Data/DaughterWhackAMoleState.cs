using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaughterWhackAMoleState : EnemyState
{
    private GameObject[] graves;

    public DaughterWhackAMoleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        graves = FindObjectOfType<GraveContainer>().graves;
    }
    public override void ExitState() { 
        base.ExitState();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
}
