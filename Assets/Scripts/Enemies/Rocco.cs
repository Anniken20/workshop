using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocco : Enemy
{
    [HideInInspector] public EnemyShootState shootState;

    private void Awake()
    {
        base.MyAwake();

        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        shootState = gameObject.AddComponent<EnemyShootState>();
        shootState.Initialize(this, stateMachine);

        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        //set default state
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentEnemyState?.PhysicsUpdate();
    }

    public void BeginBattle()
    {
        stateMachine.ChangeState(shootState);
    }

    public void BeginBattleDelay(float seconds)
    {
        Invoke(nameof(BeginBattle), 8f);
    }
}
