using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureKeeper : Enemy
{
    public GameObject yellowModel;
    public GameObject normalModel;

    public GameObject[] boxes;
    private float explosionPower = 20000f;
    public Animator anim;

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
        } else
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Scared", false);
            anim.SetBool("Throw", true);
            Debug.Log("Set Throwing to true");
            StartCoroutine(ThrowDelay());

            foreach (GameObject box in boxes)
            {
                Rigidbody rb = box.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce((box.transform.position - transform.position).normalized
                        * explosionPower, ForceMode.Impulse);
                    //Debug.Log("sent " + box + " flying");
                }
            }
        }
    }

    public IEnumerator YellowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        yellowModel.SetActive(false);
        normalModel.SetActive(true);
    }

    public IEnumerator ThrowDelay()
    {
        Debug.Log("Waiting to go back to idle");
        yield return new WaitForSeconds(2f);
        anim.SetBool("Throw", false);
        anim.SetBool("Scared", false);
        anim.SetBool("Idle", true);
        Debug.Log("Back to idle");
    }


}
