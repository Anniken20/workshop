using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santana : Enemy
{
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyLobAttackState lobAttackState;
    [HideInInspector] public EnemyAOEAttackState AOEAttackState;
    
    private void Awake()
    {
        base.MyAwake();

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        lobAttackState = gameObject.AddComponent<EnemyLobAttackState>();
        lobAttackState.Initialize(this, stateMachine);

        AOEAttackState = gameObject.AddComponent<EnemyAOEAttackState>();
        AOEAttackState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(pacingState);
    }

    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
    }
    
    public void StopPacing()
    {
        stateMachine.ChangeState(AOEAttackState);
    }

    public void BeginLob()
    {
        stateMachine.ChangeState(lobAttackState);
    }

    public void StopLob()
    {
        stateMachine.ChangeState(AOEAttackState);
    }

    public void BeginAOE()
    {
        stateMachine.ChangeState(AOEAttackState);
    }

    public void StopAOE()
    {
        stateMachine.ChangeState(lobAttackState);
    }

}

