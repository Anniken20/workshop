using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinecartMechanic : OnPlayerHit
{
    public override void HitEffect(Collision col)
    {
        Debug.Log("Stepped on minecart");
        col.transform.SetParent(transform);
        transform.DOMoveZ(transform.position.z - 10f, 2f);
    }
}
