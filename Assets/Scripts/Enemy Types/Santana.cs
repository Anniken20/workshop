using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santana : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyAttackState attackState;
    [HideInInspector] public EnemyPacingState pacingState;

    private void Awake()
    {
        base.MyAwake();
        idleState = new EnemyIdleState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        pacingState = new EnemyPacingState(this, stateMachine);

        //set default state
        stateMachine.Initialize(pacingState);
    }

    public void InIdleRange()
    {
        stateMachine.ChangeState(idleState);
    }

    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
    }
}

