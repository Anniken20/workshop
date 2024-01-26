using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleFloor : OnPlayerHit
{
    [SerializeField] float breakDelay;

     public override void HitEffect(Collision other){
        //Maybe Add some shake animation here?

        //^^
        Destroy(gameObject, breakDelay);
    }
}
