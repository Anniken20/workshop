using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DuelEnemy : Enemy
{
    public int phase = 1;
    [HideInInspector] public DuelState duelState;

    public void GoNextPhase()
    {
        phase++;
        StartPhase(phase);
    }

    public void StartDuel()
    {
        stateMachine.ChangeState(duelState);
    }

    public virtual void PlayerWonDuel()
    {

    }

    public virtual void EnemyWonDuel()
    {

    }

    protected abstract void StartPhase(int ph);
}
