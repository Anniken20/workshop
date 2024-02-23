using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salesman : Enemy
{
    [HideInInspector] public SalesmanBulletState bulletState;
    [HideInInspector] public EnemyPacingState pacingState;
    private void Awake()
    {
        base.MyAwake();
        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);
        bulletState = gameObject.AddComponent<SalesmanBulletState>();
        bulletState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);

    }
    public void Pacing()
    {
        stateMachine.ChangeState(pacingState);
    }
    public void ExitPacing()
    {
        stateMachine.ChangeState(idleState);
    }
    public void BulletState()
    {
        stateMachine.ChangeState(bulletState);
    }
    public void BulletLightState()
    {
        stateMachine.ChangeState(pacingState);
    }
}
