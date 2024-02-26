using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daughter : Enemy
{
    [HideInInspector] DaughterWhackAMoleState wamState;
    [HideInInspector] GraveSelectionState graveSelection;
    private void Awake()
    {
        base.MyAwake();
        wamState = gameObject.AddComponent<DaughterWhackAMoleState>();
        wamState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        graveSelection = gameObject.AddComponent<GraveSelectionState>();
        graveSelection.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
    }
    public void EnterWAM()
    {
        stateMachine.ChangeState(wamState);
    }
    public void ExitWAM()
    {
        stateMachine.ChangeState(graveSelection);
    }
    public void Idle()
    {
        stateMachine.ChangeState(idleState);
    }
    public void ExitIdle()
    {
        stateMachine.ChangeState(wamState);
    }
    public void SelectGrave()
    {
        stateMachine.ChangeState(graveSelection);
    }
    public void ExitGraveSelection()
    {
        stateMachine.ChangeState(wamState);
    }
}
