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
    
    public void StopPacing()
    {
        stateMachine.ChangeState(shootState);
    }

    public void BeginLob()
    {
        stateMachine.ChangeState(lobAttackState);
    }
    
    public void StopLob()
    {
        stateMachine.ChangeState(shootState);
    }

    public void StartShooting()
    {
        stateMachine.ChangeState(shootState);
    }

    public void StopShooting()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void BeginEvade()
    {
        stateMachine.ChangeState(evadeState);
    }
    
    public void StopEvade()
    {
        stateMachine.ChangeState(pacingState);
    }
}

