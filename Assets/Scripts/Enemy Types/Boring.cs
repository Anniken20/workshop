using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boring : Enemy
{
    private void Awake()
    {
        
    }

    public void InIdleRange()
    {
        stateMachine.ChangeState(idleState);
    }

    public void BeginPacing()
    {
        stateMachine.ChangeState(pacingState);
    }
}
