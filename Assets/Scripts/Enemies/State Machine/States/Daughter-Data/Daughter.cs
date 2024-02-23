using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daughter : Enemy
{
    [HideInInspector] DaughterWhackAMoleState wamState;
    private void Awake()
    {
        //base.Awake();
        wamState = gameObject.AddComponent<DaughterWhackAMoleState>();
        wamState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);
    }
    public void EnterWAM()
    {

    }
    public void ExitWAM()
    {

    }
    public void Idle()
    {

    }
    public void ExitIdle()
    {

    }
}
