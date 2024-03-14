using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseWrangling : LassoWrangle
{
    private Horse h;
    HorseChargeTrigger chargeTrigger;
    private Animator anim;
    private void Start(){
        h = GetComponentInParent<Horse>();
        anim = h.GetComponent<Animator>();
        //if(h.animator != null) h.animator.SetBool("BeingWrangled", true);
        chargeTrigger = FindObjectOfType<HorseChargeTrigger>();
    }
    public override void WinMiniGame(){
        base.WinMiniGame();
        StopCoroutine(lossRoutine);
        Debug.Log("U win :D");     
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
        h.Charge();
        h.isStunned = false;
        //h.currentHealth -= 35;
        h.TakeDamage(35);
        if(h.animator != null) h.animator.SetBool("BeingWrangled", false);
        anim.SetBool("BeingWrangled", false);
        if(h.animator != null) h.animator.SetBool("Idle", true);
        anim.SetBool("Idle", true);
        if (chargeTrigger != null)
        {
            chargeTrigger.wrangling = false;
        }
    }
    public override void LoseMiniGame(){
        base.LoseMiniGame();
        Debug.Log("U Lose D:");
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
        h.Charge();
        h.isStunned = false;
        //h.currentHealth += 15;
        if(h.animator != null) h.animator.SetBool("BeingWrangled", false);
        anim.SetBool("BeingWrangled", false);
        if(h.animator != null) h.animator.SetBool("Idle", true);
        anim.SetBool("Idle", true);
        if (chargeTrigger != null)
        {
            chargeTrigger.wrangling = false;
        }
    }
    public override void StartedWrangling()
    {
        if(chargeTrigger != null) 
        {
            chargeTrigger.wrangling = true;
            anim.SetBool("BeingWrangled", true);
            anim.SetBool("Stunned", false);
        }
    }
    public override void Update(){
        base.Update();
        if(h.currentHealth <= 0){
            WinMiniGame();
            anim.enabled = true;
            anim.SetBool("Dead", true);
            anim.SetBool("BeingWrangled", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Running", false);
            anim.SetBool("Stunned", false);
            this.gameObject.SetActive(false);
        }
    }
}
