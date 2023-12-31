using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheriff : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyShootState shootState;
    [HideInInspector] public EnemyHidingState hidingState;

    public Animator animator;

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

        animator = gameObject.GetComponent<Animator>();
    }

    public void runaway()
    {
        stateMachine.ChangeState(hidingState);
        animator.SetBool("isRunning", true);
        animator.SetBool("isShooting", false);
    }
    public void stoprun()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", true);
    }

    public void startshooting()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", true);
    }
    public void stopshooting()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("isRunning", true);
        animator.SetBool("isShooting", false);
    }

    public void idle()
    {
        stateMachine.ChangeState(idleState);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
    }

    public void stopidle()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("isRunning", true);
        animator.SetBool("isShooting", false);
    }

    public void pacing()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("isRunning", true);
        animator.SetBool("isShooting", false);
    }

    public void stoppacing()
    {
        stateMachine.ChangeState(idleState);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
    }
}

