using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    public void OpenGate()
    {
        transform.DORotate(new Vector3(0, 90, 0), 1.0f);
    }
}