using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    public float xDegrees;
    public float yDegrees;
    public float zDegrees;

    public void OpenGate(float time = 1.0f)
    {
        transform.DORotate(new Vector3(yDegrees, xDegrees, zDegrees), time);
    }
}