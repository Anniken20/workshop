using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    public float duration;
    public float xDelta;
    public float yDelta;
    public float zDelta;
    public void RotateMe()
    {
        transform.DORotate(new Vector3(xDelta, yDelta, zDelta), duration, RotateMode.LocalAxisAdd);
    }

    public void RotateBouncey()
    {
        transform.DORotate(new Vector3(xDelta, yDelta, zDelta), duration, RotateMode.LocalAxisAdd).SetEase(Ease.OutBounce);
    }
}
