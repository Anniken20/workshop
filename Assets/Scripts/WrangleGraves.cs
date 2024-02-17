using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrangleGraves : LassoWrangle
{
    public GameObject grave;
    public GameObject spawner;
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
        Invoke("Rotate", .1f);
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

    private void Rotate()
    {
        grave.transform.Rotate(0f, 0f, 180f, Space.World);
        spawner.SetActive(false);
    }
}
