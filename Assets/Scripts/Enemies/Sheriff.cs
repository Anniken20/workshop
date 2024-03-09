using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheriff : DuelEnemy
{
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyShootState shootState;
    [HideInInspector] public EnemyHidingState hidingState;
    [HideInInspector] public HorseChargeState chargeState;
    [HideInInspector] public RunToPointsState runToPointsState;
    [HideInInspector] public KnockbackState knockbackState;
    [HideInInspector] public EnemyThrowState throwState;
    [HideInInspector] public HorseFreezeState freezeState;
    [HideInInspector] public HorseStunnedState stunnedState;

    private bool isStunned;
    private bool isCharging;

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

        chargeState = gameObject.AddComponent<HorseChargeState>();
        chargeState.Initialize(this, stateMachine);

        runToPointsState = gameObject.AddComponent<RunToPointsState>();
        runToPointsState.Initialize(this, stateMachine);

        knockbackState = gameObject.AddComponent<KnockbackState>();
        knockbackState.Initialize(this, stateMachine);

        throwState = gameObject.AddComponent<EnemyThrowState>();
        throwState.Initialize(this, stateMachine);

        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        freezeState = gameObject.AddComponent<HorseFreezeState>();
        freezeState.Initialize(this, stateMachine);

        stunnedState = gameObject.AddComponent<HorseStunnedState>();
        stunnedState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(idleState);

        animator = gameObject.GetComponent<Animator>();
    }

    #region OldBehavior

    public void runaway()
    {
        stateMachine.ChangeState(hidingState);
    }
    public void stoprun()
    {
        stateMachine.ChangeState(shootState);
    }

    public void startshooting()
    {
        stateMachine.ChangeState(shootState);
    }
    public void stopshooting()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void idle()
    {
        stateMachine.ChangeState(idleState);
    }

    public void stopidle()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void pacing()
    {
        stateMachine.ChangeState(pacingState);
    }

    public void stoppacing()
    {
        stateMachine.ChangeState(idleState);
    }

    #endregion

    #region HorseBehavior

    public void StartChargeBattle()
    {
        Invoke(nameof(Charge), 11f);
    }
    public void Charge()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isCharging = true;
        stateMachine.ChangeState(chargeState);
    }
    public void StopCharge()
    {
        isCharging = false;
        stateMachine.ChangeState(pacingState);
    }

    public void Stunned()
    {
        isStunned = true;
        //Debug.Log("Petah the horse is stunned");
        stateMachine.ChangeState(stunnedState);
    }
    public void ExitStuned()
    {
        isStunned = false;
        stateMachine.ChangeState(pacingState);
    }
    public void Idle()
    {
        stateMachine.ChangeState(idleState);
    }
    public void StopIdle()
    {
        stateMachine.ChangeState(pacingState);
    }
    public void Pacing()
    {
        stateMachine.ChangeState(pacingState);
    }
    public void StopPacing()
    {
        stateMachine.ChangeState(idleState);
    }
    public void Freeze()
    {
        stateMachine.ChangeState(freezeState);
    }
    public void StopFreeze()
    {
        stateMachine.ChangeState(idleState);
    }
    #endregion

    protected override void StartPhase(int ph)
    {
        
    }
}

