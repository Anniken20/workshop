using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class DuelCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 cameraOffsetDelta;

    //private variables ------------------
    private Vector3 baseCameraPosition;
    private CinemachineVirtualCamera vcam;
    private Tween moveTween;
    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        baseCameraPosition = transposer.m_TrackedObjectOffset;
    }
    public void StartDuel(float approximateTime)
    {
        float duration = approximateTime + Random.Range(1f, 3f);
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        moveTween.SetUpdate(true);
        moveTween = DOTween.To(() => transposer.m_TrackedObjectOffset, x => transposer.m_TrackedObjectOffset = x,
            cameraOffsetDelta + baseCameraPosition, duration);
        //Invoke(nameof(Reset), duration);
    }

    public void Reset()
    {
        if (moveTween != null) moveTween.Kill();
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset = baseCameraPosition;
    }
}
