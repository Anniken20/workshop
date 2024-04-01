using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using DG.Tweening;
public class TeleportPlayerToCage : MonoBehaviour
{
    public GameObject cage;
    public void StartCage()
    {
        ThirdPersonController.Main.InstantTeleport(transform.position);
        cage.transform.DOMove(transform.position, 1f).SetEase(Ease.OutBounce);
        GetComponent<CageTrap>().ActivateTrap();
    }

    public void StopCage()
    {
        GetComponent<CageTrap>().DeactivateTrap();
        cage.transform.DOMoveY(transform.position.y + 100f, 3f).SetEase(Ease.InCubic);
    }
}
