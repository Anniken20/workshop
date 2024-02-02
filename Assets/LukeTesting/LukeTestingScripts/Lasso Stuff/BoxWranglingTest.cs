using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWranglingTest : LassoWrangle
{
    public override void WinMiniGame(){
        StopCoroutine(lossRoutine);
        Debug.Log("U win :D");
        StartCoroutine(EnableDelay());        
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }

    public override void LoseMiniGame(){
        Debug.Log("U Lose D:");
        StartCoroutine(EnableDelay());        
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }
}
