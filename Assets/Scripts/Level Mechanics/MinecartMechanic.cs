using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using StarterAssets;

public class MinecartMechanic : MonoBehaviour
{
    private SplineAnimate splineAnimator;

    //start and stop for a frame to fix rotation bug
    private void Start()
    {
        splineAnimator = GetComponent<SplineAnimate>();
        splineAnimator.Updated += OnUpdateSpline;
    }

    public void OnUpdateSpline(Vector3 pos, Quaternion rotation)
    {
        if(splineAnimator.NormalizedTime > 0.99f)
        {
            splineAnimator.NormalizedTime = 1f;
            OnEnd();
        }
    }

    public void HitWall()
    {
        
    }

    public void OnEnd()
    {

    }
 
}
