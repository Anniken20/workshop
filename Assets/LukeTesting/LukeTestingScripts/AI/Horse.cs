using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Enemy
{
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public EnemyStunnedState stunnedState;
    [HideInInspector] public HorseChargeState chargeState;

    private void Awake(){
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        chargeState = gameObject.AddComponent<HorseChargeState>();
        chargeState.Initialize(this, stateMachine);

        stunnedState = gameObject.AddComponent<EnemyStunnedState>();
        stunnedState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    public void Charge(){
        stateMachine.ChangeState(chargeState);
    }
    public void StopCharge(){
        stateMachine.ChangeState(pacingState);
    }
    public void Stunned(){
        stateMachine.ChangeState(stunnedState);
    }
    public void ExitStuned(){
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
}
