using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartHitController : MonoBehaviour
{
    public MinecartMechanic minecartMechanic;
    private void OnTriggerEnter(Collider other)
    {
        MinecartWall mcw = other.GetComponent<MinecartWall>();
        if (mcw != null)
        {
            minecartMechanic.HitWall();
            mcw.OnHit();
        }
    }
}
