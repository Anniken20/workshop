using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public HorseStunnedState stunnedState;
    [HideInInspector] public HorseChargeState chargeState;
    [HideInInspector] public HorseFreezeState freezeState;
    [HideInInspector] public bool isCharging;

    //public GameObject stunnedText;
    //public GameObject chargingText;

    //private Enemy e;
    [HideInInspector] public bool isStunned;

    private void Awake(){

        //e = GetComponentInParent<Enemy>();

        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        chargeState = gameObject.AddComponent<HorseChargeState>();
        chargeState.Initialize(this, stateMachine);

        stunnedState = gameObject.AddComponent<HorseStunnedState>();
        stunnedState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        freezeState = gameObject.AddComponent<HorseFreezeState>();
        freezeState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    public void Charge(){
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isCharging = true;
        stateMachine.ChangeState(chargeState);
    }
    public void StopCharge(){
        isCharging = false;
        stateMachine.ChangeState(pacingState);
    }
    public void Stunned(){
        isStunned = true;
        //Debug.Log("Petah the horse is stunned");
        stateMachine.ChangeState(stunnedState);
    }
    public void ExitStuned(){
        isStunned = false;
        stateMachine.ChangeState(pacingState);
    }
    public void Idle(){
        stateMachine.ChangeState(idleState);
    }
    public void StopIdle(){
        stateMachine.ChangeState(pacingState);
    }
    public void Pacing(){
        stateMachine.ChangeState(pacingState);
    }
    public void StopPacing(){
        stateMachine.ChangeState(idleState);
    }
    public void Freeze(){
        stateMachine.ChangeState(freezeState);
    }
    public void StopFreeze(){
        stateMachine.ChangeState(idleState);
    }
}
