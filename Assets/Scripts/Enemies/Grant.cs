using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Events;

public class Grant : Enemy
{
    public bool shootsInsteadOfThrows = false;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyThrowState throwState;
    [HideInInspector] public KnockbackState knockbackState;
    [HideInInspector] public EnemyShootState shootState;

    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        throwState = gameObject.AddComponent<EnemyThrowState>();
        throwState.Initialize(this, stateMachine);

        knockbackState = gameObject.AddComponent<KnockbackState>();
        knockbackState.Initialize(this, stateMachine);

        shootState = gameObject.AddComponent<EnemyShootState>();
        shootState.Initialize(this, stateMachine);

        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState?.PhysicsUpdate();
    }

    public void BeginBattle()
    {
        if (shootsInsteadOfThrows)
        {
            stateMachine.ChangeState(shootState);
        }
        else
        {
            stateMachine.ChangeState(throwState);
        }
        
    }

    public void KnockawayPlayer()
    {
        
        stateMachine.ChangeState(knockbackState);
    }

    public void ResumeThrowing()
    {
        if (shootsInsteadOfThrows)
        {
            stateMachine.ChangeState(shootState);
        }
        else
        {
            stateMachine.ChangeState(throwState);
        }
        
    }

}
