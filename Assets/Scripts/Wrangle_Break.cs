using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wrangle_Break : LassoWrangle
{
    public BreakController breaker;
    public UnityEvent wrangleComplete;
    private Collider c;
    public AudioClip breakSound;
    private void Start()
    {
       c = GetComponent<Collider>();

    }
    public override void WinMiniGame()
    {
        StopCoroutine(lossRoutine);
        Debug.Log("U win :D");
        StartCoroutine(EnableDelay());
        wrangleComplete?.Invoke();
        c.enabled = false;
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
        player.canLasso = true;
        controller._manipulatingLasso = false;
        AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, breakSound);
        breaker.BreakIntoPieces();
        
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
}
