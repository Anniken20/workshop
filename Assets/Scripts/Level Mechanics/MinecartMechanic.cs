using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Splines;

public class MinecartMechanic : OnPlayerHit
{
    private SplineAnimate splineAnimator;
    public override void HitEffect(Collision col)
    {
        Debug.Log("Stepped on minecart");
        splineAnimator = GetComponent<SplineAnimate>();
        col.transform.SetParent(transform);
        //transform.DOMoveZ(transform.position.z - 10f, 2f);

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
}
