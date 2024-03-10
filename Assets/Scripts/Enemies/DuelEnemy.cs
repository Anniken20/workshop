using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DuelEnemy : Enemy
{
    [Header("Duel Enemy Variables")]
    public int phase = 1;
    

    public void GoNextPhase()
    {
        phase++;
        StartPhase(phase);
    }

    public virtual void PlayerWonDuel()
    {
        switch (phase)
        {
            case 1:
                GoNextPhase();
                break;
            case 2:
                GoNextPhase();
                break;
            case 3:
                Die();
                break;
        }
    }

    public virtual void EnemyWonDuel()
    {

    }

    public override void TakeDamage(int delta)
    {
        currentHealth -= delta;
        damageDelegate?.Invoke();
    }

    protected abstract void StartPhase(int ph);
}
