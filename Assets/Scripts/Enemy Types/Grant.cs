using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.Events;

public class Grant : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyThrowState throwState;

    public Animator animator;

    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        throwState = gameObject.AddComponent<EnemyThrowState>();
        throwState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(throwState);

        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState?.PhysicsUpdate();
    }

}
