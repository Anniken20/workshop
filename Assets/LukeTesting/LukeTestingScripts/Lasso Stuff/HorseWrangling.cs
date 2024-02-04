using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseWrangling : LassoWrangle
{
    private Horse h;
    private void Start(){
        h = GetComponentInParent<Horse>();
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
        h.currentHealth += 15;
        
        
    }
}
