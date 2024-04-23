using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class StopSplineBeforeEnd : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    private float normalizedEndThreshold = 0.98f;
    // Start is called before the first frame update
    void Start()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        if(splineAnimate.NormalizedTime > normalizedEndThreshold)
        {
            splineAnimate.Pause();
        }
    }
}
