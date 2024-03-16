using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeper : Enemy
{
    private void Awake(){
        base.MyAwake();
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
    }
    public void Idle(){
        stateMachine.ChangeState(idleState);
    }
    public void ExitIdle(){

    }
}
