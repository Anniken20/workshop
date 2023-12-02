using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : Enemy
{
    [HideInInspector] public EnemyLobAttackState lobAttackState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyShootState shootState;
    [HideInInspector] public EnemyEvadeState evadeState;
    [HideInInspector] public EnemyIdleState idleState;

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

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        
        //set default state
        stateMachine.Initialize(idleState);
        animator = gameObject.GetComponent<Animator>();

        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }
    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("Moving", true);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }
    
    public void StopPacing()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }

    public void BeginLob()
    {
        stateMachine.ChangeState(lobAttackState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", true);
    }
    
    public void StopLob()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }

    public void StartShooting()
    {
        stateMachine.ChangeState(shootState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", true);
        animator.SetBool("Throwing", false);
    }

    public void StopShooting()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }

    public void BeginEvade()
    {
        stateMachine.ChangeState(evadeState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }
    
    public void StopEvade()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }
        public void idle()
    {
        stateMachine.ChangeState(idleState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }

    public void stopidle()
    {
        stateMachine.ChangeState(pacingState);
        animator.SetBool("Moving", false);
        animator.SetBool("Shooting", false);
        animator.SetBool("Throwing", false);
    }
}

