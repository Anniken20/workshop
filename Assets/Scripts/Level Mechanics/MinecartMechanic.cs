using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using StarterAssets;

public class MinecartMechanic : MonoBehaviour
{
    private SplineAnimate splineAnimator;
    public GameObject riderTransform;
    private CharacterController characterController;
    private Coroutine rideRoutine;

    //start and stop for a frame to fix rotation bug
    private void Start()
    {
        splineAnimator = GetComponent<SplineAnimate>();
    }

    public void StartRide()
    {
        splineAnimator.Play();
        rideRoutine = StartCoroutine(RideRoutine());
        //ThirdPersonController.Main.GetComponent<GunController>().EnterMinecartMode();
    }

    private IEnumerator RideRoutine()
    {
        characterController = ThirdPersonController.Main.GetComponent<CharacterController>();
        characterController.enabled = false;
        while (true)
        {
            yield return null;
            ThirdPersonController.Main.transform.position = riderTransform.transform.position;
        }
    }

    public void OnUpdateSpline(Vector3 pos, Quaternion rotation)
    {
        if(splineAnimator.NormalizedTime > 0.99f)
        {
            splineAnimator.NormalizedTime = 1f;
            OnEnd();
        }
    }

    public void CheckSplineTime()
    {
        if (splineAnimator.NormalizedTime > 0.99f)
        {
            splineAnimator.NormalizedTime = 1f;
            OnEnd();
        }
    }

    public void HitWall()
    {
        
    }

    public void EndRide()
    {
        OnEnd();
        splineAnimator.Pause();
        //ThirdPersonController.Main.GetComponent<GunController>().ExitMinecartMode();
    }

    public void KickOffPlayer()
    {
        OnEnd();
        characterController = ThirdPersonController.Main.GetComponent<CharacterController>();
        characterController.enabled = true;
    }

    public void OnEnd()
    {
        StopCoroutine(rideRoutine);
    }

    public void SwitchTracks(GameObject newTrack)
    {
        SplineContainer spl = newTrack.gameObject.GetComponent<SplineContainer>();
        if (spl == null) Debug.LogWarning("No spline component on " + newTrack.name);
        else
        {
            SwitchTracks(spl);
            //splineAnimator.Container = spl;
            //splineAnimator.NormalizedTime = 0;
            /*
            float theTime;
            SplineUtility.GetNearestPoint<Spline>(splineAnimator.Container.Spline,
                ThirdPersonController.Main.transform.position,
                out _,
                out theTime);
            splineAnimator.NormalizedTime = theTime;
            splineAnimator.enabled = true;
            */
        }
    }

    public void SwitchTracks(SplineContainer spl)
    {
        if (spl == null) { }
        else
        {
            float newDuration = spl.CalculateLength() /
             splineAnimator.Container.CalculateLength() * splineAnimator.Duration;

            splineAnimator.Container = spl;
            splineAnimator.Duration = newDuration;
            splineAnimator.NormalizedTime = 0;
        }
    }
 
}
