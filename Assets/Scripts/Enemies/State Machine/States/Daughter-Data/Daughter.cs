using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daughter : Enemy
{
    [HideInInspector] DaughterWhackAMoleState wamState;
    [HideInInspector] EnemyShootState shootState;
    private void Awake()
    {
        base.MyAwake();
        wamState = gameObject.AddComponent<DaughterWhackAMoleState>();
        wamState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        shootState = gameObject.AddComponent<EnemyShootState>();
        shootState.Initialize(this, stateMachine);
        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
    }
    public void EnterWAM()
    {
        stateMachine.ChangeState(wamState);
    }
    public void ExitWAM()
    {
        stateMachine.ChangeState(idleState);
    }
    public void Idle()
    {
        stateMachine.ChangeState(idleState);
    }
    public void ExitIdle()
    {
        stateMachine.ChangeState(wamState);
    }
    public void Shooting()
    {
        stateMachine.ChangeState(shootState);
    }
    public void StopShooting()
    {
        stateMachine.ChangeState(idleState);
    }
}
