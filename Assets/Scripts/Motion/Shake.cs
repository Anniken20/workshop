using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shake : MonoBehaviour
{
    [Tooltip("If no reference is attached here, this object will shake")]
    public GameObject target;
    public float intensity;
    public float duration;

    private void Start()
    {
        if (target == null) target = gameObject;
    }

    public void ShakeIt()
    {
        target.transform.DOShakePosition(intensity, duration);
    }

    public void ShakeForOneTenthSecond(float newIntensity)
    {
        target.transform.DOShakePosition(newIntensity, 2f);
    }

    public void ShakeForOneSecond(float newIntensity)
    {
        target.transform.DOShakePosition(newIntensity, 1f);
    }
}
