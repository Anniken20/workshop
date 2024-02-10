using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : Pickup
{
    public float hoverHeight;

    private float timeUntilHover = 1f;
    private bool hit;

    protected override void PickupAction()
    {
        base.PickupAction();
        CoinCollector.Instance.CollectCoin();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit) StartCoroutine(HitGround());
    }

    private IEnumerator HitGround()
    {
        hit = true;
        yield return new WaitForSeconds(timeUntilHover);
        Destroy(GetComponent<Rigidbody>());
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitData, Mathf.Infinity);
        float height = hitData.point.y;
        transform.DOMoveY(hoverHeight + height, timeUntilHover).SetEase(Ease.InOutCubic);
        transform.DORotate(new Vector3(0, 0, 0), timeUntilHover).SetEase(Ease.InOutCubic);

        yield return new WaitForSeconds(timeUntilHover);
        GetComponent<Hover>().StartHovering();
        GetComponent<RotateOverTime>().StartRotating();
    }
}
