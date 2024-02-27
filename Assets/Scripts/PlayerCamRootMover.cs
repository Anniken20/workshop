using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamRootMover : MonoBehaviour
{
    private Vector3 localOffset;
    private Tween camTween;
    private void Start()
    {
        localOffset = transform.localPosition;
    }
    public void ResetPosition()
    {
        camTween = transform.DOLocalMove(localOffset, 1f);
    }
}
