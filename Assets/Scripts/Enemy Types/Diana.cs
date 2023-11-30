using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : Enemy
{
    [HideInInspector] public EnemyLobAttackState lobAttackState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyShootState shootState;
    [HideInInspector] public EnemyEvadeState evadeState;

    [Header("Diana specific")]
    public Animator animator;

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
        animator.SetBool("moving", true);
    }
    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("moving", true);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }
    
    public void StopPacing()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("moving", false);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }

    public void BeginLob()
    {
        stateMachine.ChangeState(lobAttackState);
        animator.SetBool("moving", false);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", true);
        animator.SetBool("stunned", false);
    }
    
    public void StopLob()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("moving", false);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }

    public void StartShooting()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("moving", false);
        animator.SetBool("shooting", true);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }

    public void StopShooting()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("moving", false);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }

    public void BeginEvade()
    {
        stateMachine.ChangeState(evadeState);
        animator.SetBool("moving", true);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }
    
    public void StopEvade()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("moving", false);
        animator.SetBool("shooting", false);
        animator.SetBool("throwing", false);
        animator.SetBool("stunned", false);
    }
}

