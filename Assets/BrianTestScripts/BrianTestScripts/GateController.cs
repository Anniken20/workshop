using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    public float xDegrees;
    public float yDegrees;
    public float zDegrees;

    public void OpenGate(float time)
    {
        transform.DORotate(new Vector3(xDegrees, yDegrees, zDegrees), time, RotateMode.WorldAxisAdd);
    }

}