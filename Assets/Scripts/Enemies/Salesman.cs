using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salesman : Enemy
{
    [HideInInspector] public SalesmanBulletState bulletState;
    [HideInInspector] public SalesmanChasePlayerState chaseState;
    [HideInInspector] public EnemyPacingState pacingState;
    private void Awake()
    {
        base.MyAwake();
        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);
        bulletState = gameObject.AddComponent<SalesmanBulletState>();
        bulletState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        chaseState = gameObject.AddComponent<SalesmanChasePlayerState>();
        chaseState.Initialize(this, stateMachine);
        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);

    }
    public void Pacing()
    {
        stateMachine.ChangeState(pacingState);
    }
    public void ExitPacing()
    {
        stateMachine.ChangeState(idleState);
    }
    public void BulletState()
    {
        stateMachine.ChangeState(bulletState);
    }
    public void ExitBulletState()
    {
        stateMachine.ChangeState(pacingState);
    }
    public void ChaseState()
    {
        stateMachine.ChangeState(chaseState);
    }
    public void ExitChase()
    {
        stateMachine.ChangeState(idleState);
    }
}
