using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeper : Enemy
{
    private void Awake(){
        base.MyAwake();
        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
    }
    public void Idle(){
        stateMachine.ChangeState(idleState);
    }
    public void ExitIdle(){

    }

    public override void OnShot(BulletController bullet)
    {
        base.OnShot(bullet);
        damageDelegate?.Invoke();
    }
}
