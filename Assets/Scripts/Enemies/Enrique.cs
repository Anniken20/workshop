using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Events;

public class Enrique : Enemy, ICryable
{
    [HideInInspector] public CryState cryState;
    [HideInInspector] public RunToPointsState runToPointsState;
    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        cryState = gameObject.AddComponent<CryState>();
        cryState.Initialize(this, stateMachine);

        runToPointsState = gameObject.AddComponent<RunToPointsState>();
        runToPointsState.Initialize(this, stateMachine);

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
        stateMachine.ChangeState(runToPointsState);
    }

    public void StartCrying()
    {
        stateMachine.ChangeState(cryState);
    }

    
    public override void OnShot(BulletController bullet)
    {
        if (stateMachine.CurrentEnemyState is CryState)
        {
            TakeDamage((int)bullet.currDmg);
            stateMachine.ChangeState(runToPointsState);
        }
    }
}
