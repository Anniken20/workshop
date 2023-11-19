using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheriff : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyShootState shootState;
    [HideInInspector] public EnemyHidingState hidingState;

    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        shootState = gameObject.AddComponent<EnemyShootState>();
        shootState.Initialize(this, stateMachine);

        hidingState = gameObject.AddComponent<EnemyHidingState>();
        hidingState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(idleState);
    }

    public void runaway()
    {
        stateMachine.ChangeState(hidingState);
    }
    public void stoprun()
    {
        stateMachine.ChangeState(shootState);
    }

    public void startshooting()
    {
        stateMachine.ChangeState(shootState);
    }
    public void stopshooting()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void idle()
    {
        stateMachine.ChangeState(idleState);
    }

    public void stopidle()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void pacing()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void stoppacing()
    {
        stateMachine.ChangeState(idleState);
    }
}

