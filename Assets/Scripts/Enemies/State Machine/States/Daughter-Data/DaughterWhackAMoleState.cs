using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaughterWhackAMoleState : EnemyState
{
    private DaughterData daughterData;
    public DaughterWhackAMoleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        daughterData = (DaughterData)enemy.FindData("DaughterData");
        Debug.Log("Entering Whack a mole state");
        this.transform.position = daughterData.selectedGrave.transform.position;
        daughterData.selectedGrave = null;
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
