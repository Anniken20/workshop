using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineTrackSwitch : MonoBehaviour
{
    [SerializeField] private SplineAnimate rider;
    [SerializeField] private SplineContainer fromSpline;
    [SerializeField] private SplineContainer toSpline;

    private void OnTriggerEnter(Collider other)
    {
        //if there's an enemy on the ride
        if(other.gameObject.GetComponent<Enemy>() != null)
        {
            SwitchSplines();            
        }
    }

    public void SwitchSplines()
    {
        float newDuration = toSpline.CalculateLength() /
            fromSpline.CalculateLength() * rider.Duration;

        rider.Container = toSpline;
        rider.Duration = newDuration;
        rider.ElapsedTime = 0f;
    }
}
