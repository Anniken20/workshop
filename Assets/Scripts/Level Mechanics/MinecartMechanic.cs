using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;
using StarterAssets;

public class MinecartMechanic : OnPlayerHit
{
    private SplineAnimate splineAnimator;
    private ThirdPersonController playerCon;
    public override void HitEffect(Collision col)
    {
        Debug.Log("Stepped on minecart");
        splineAnimator = GetComponent<SplineAnimate>();
        //col.transform.SetParent(transform);
        //transform.DOMoveZ(transform.position.z - 10f, 2f);

        StartCoroutine(MovePlayerWithMinecartRoutine());

        splineAnimator.Play();
    }

    //kick the player off
    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(null);
        }
    }

    private IEnumerator MovePlayerWithMinecartRoutine()
    {
        while (true) {
            playerCon.SetMotion(Vector3.zero);

            //wait a frame before resuming
            yield return null;

            //check if reached end of track
            if(splineAnimator.NormalizedTime > 0.99f)
            {
                yield break;
            }
        }
    }
}
