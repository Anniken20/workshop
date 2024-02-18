using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socialite : Enemy
{
    [HideInInspector] public SocialiteMoveState moveState;
    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
        moveState = gameObject.AddComponent<SocialiteMoveState>();
        moveState.Initialize(this, stateMachine);
        stateMachine.Initialize(idleState);
    }
    public void Idle()
    {
        stateMachine.ChangeState(idleState);
    }
    public void StopIdle()
    {
        stateMachine.ChangeState(moveState);
    }
    public void StartMove()
    {
        stateMachine.ChangeState(moveState);
    }
    public void StopMove()
    {
        stateMachine.ChangeState(idleState);
    }

}
