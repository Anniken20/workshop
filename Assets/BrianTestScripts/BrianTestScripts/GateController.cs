using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateController : MonoBehaviour
{
    public void OpenGate()
    {
        // Rotate the gate using DOTween (you should import DOTween if not already done)
        transform.DORotate(new Vector3(0, 90, 0), 1.0f);
    }
}