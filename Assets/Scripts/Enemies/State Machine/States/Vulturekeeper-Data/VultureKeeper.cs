using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeper : Enemy
{
    public GameObject yellowModel;
    public GameObject normalModel;

    private void Awake(){
        base.MyAwake();
        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);

        yellowModel.SetActive(false);
        normalModel.SetActive(true);
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
        if((int)bullet.currDmg < 200)
        {
            yellowModel.SetActive(true);
            normalModel.SetActive(false);
            StartCoroutine(YellowDelay());
        }
    }

    public IEnumerator YellowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        yellowModel.SetActive(false);
        normalModel.SetActive(true);
    }


}
