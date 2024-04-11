using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PauseRide : MonoBehaviour
{
    public bool pauseOnStart;
    private void Start()
    {
        if (pauseOnStart)
        {
            Invoke(nameof(PauseAnimate), 0.1f);
            
            
        }
    }

    public void PauseAnimate()
    {
        SplineAnimate splineAnimate = GetComponent<SplineAnimate>();
        if (splineAnimate != null)
        {
            splineAnimate.PlayOnAwake = false;
            splineAnimate.Pause();
        }
    }
}
