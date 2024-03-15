using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MinecartDamageWall : MinecartWall
{
    public int damage;
    public override void OnHit()
    {
        ThirdPersonController.Main.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
