using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrangle_Break : LassoWrangle
{
    public DamageController dmgcontrol;
    public override void WinMiniGame()
    {
        StopCoroutine(lossRoutine);
        Debug.Log("U win :D");
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
        player.canLasso = true;
        controller._manipulatingLasso = false;
        Invoke("ObjBroken", .2f);
    }

    public override void LoseMiniGame()
    {
        Debug.Log("U Lose D:");
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }

    private void ObjBroken()
    {
        dmgcontrol.ApplyDamage(100, new Vector3(.1f, .1f, .1f));
    }
}
