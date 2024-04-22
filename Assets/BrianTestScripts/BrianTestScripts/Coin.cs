using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : Pickup
{
    public float hoverHeight;
    public float magnetSpeed = 3f;

    private float collectRange = 1f;
    private float timeUntilHover = 1f;
    private bool hit;
    private GameObject moveTarget;

    protected override void PickupAction()
    {
        CoinCollector.Instance.CollectCoin();
    }

    protected internal override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            moveTarget = other.gameObject;
            StartCoroutine(MagnetRoutine());
            /*
            PickupAction();
            PlayPickupSound();
            Destroy(gameObject);
            */
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hit) StartCoroutine(HitGround());
    }

    private void ActualPickup()
    {
        StopAllCoroutines();
        PickupAction();
        //PlayPickupSound();
        Destroy(gameObject);
    }

    private IEnumerator MagnetRoutine()
    {
        Destroy(GetComponent<Rigidbody>());
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget.transform.position, Time.deltaTime * magnetSpeed);
            
            //comparing 2D distance because y-value shouldnt matter
            if(Vector2.Distance(new Vector2(transform.position.x,
                transform.position.z), new Vector2(moveTarget.transform.position.x,
                moveTarget.transform.position.z)) < collectRange)
            {
                ActualPickup();
                yield break;
            }
            yield return null;
        }
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
