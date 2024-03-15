using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WrangleLever : LassoWrangle
{
    public UnityEvent onWrangleWin;

    public override void WinMiniGame()
    {
        StopCoroutine(lossRoutine);
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;

        onWrangleWin?.Invoke();
    }
}
