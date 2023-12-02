using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santana : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyLobAttackState lobAttackState;
    [HideInInspector] public EnemyAOEAttackState AOEAttackState;
    
    public Animator animator;

    private void Awake()
    {
        base.MyAwake();
        
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        lobAttackState = gameObject.AddComponent<EnemyLobAttackState>();
        lobAttackState.Initialize(this, stateMachine);

        AOEAttackState = gameObject.AddComponent<EnemyAOEAttackState>();
        AOEAttackState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(idleState);
        animator = gameObject.GetComponent<Animator>();
    }
    public void idle()
    {
        stateMachine.ChangeState(idleState);
        animator.SetBool("Idle", true);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",false);
    }

    public void stopidle()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",false);
    }

    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",true);
    }
    
    public void StopPacing()
    {
        stateMachine.ChangeState(AOEAttackState);
        animator.SetBool("Pacing",false);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",false);
    }

    public void BeginLob()
    {
        stateMachine.ChangeState(lobAttackState);
        animator.SetBool("Pacing",false);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", true);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",false);
    }

    public void StopLob()
    {
        stateMachine.ChangeState(AOEAttackState);
        animator.SetBool("Pacing",false);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",false);
    }

    public void BeginAOE()
    {
        stateMachine.ChangeState(AOEAttackState);
        animator.SetBool("Pacing",false);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", true);
        animator.SetBool("Pacing",false);
    }

    public void StopAOE()
    {
        stateMachine.ChangeState(lobAttackState);
        animator.SetBool("Pacing",false);
        animator.SetBool("Idle", false);
        animator.SetBool("Lobbing", false);
        animator.SetBool("Moving", false);
        animator.SetBool("AOE", false);
        animator.SetBool("Pacing",false);
    }

}

