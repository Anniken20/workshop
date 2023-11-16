using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : Enemy
{
    [HideInInspector] public EnemyLobAttackState lobAttackState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyShootState shootState;
    [HideInInspector] public EnemyEvadeState evadeState;

    private void Awake()
    {
        base.MyAwake();

        lobAttackState = gameObject.AddComponent<EnemyLobAttackState>();
        lobAttackState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        shootState = gameObject.AddComponent<EnemyShootState>();
        shootState.Initialize(this, stateMachine);

        evadeState = gameObject.AddComponent<EnemyEvadeState>();
        evadeState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(pacingState);
    }
    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void BeginLob()
    {
        stateMachine.ChangeState(lobAttackState);
    }

    public void BeginShoot()
    {
        stateMachine.ChangeState(shootState);
    }

    public void BeginEvade()
    {
        stateMachine.ChangeState(evadeState);
    }
}

