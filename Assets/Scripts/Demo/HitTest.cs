using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HitTest : OnPlayerHit
{
    public override void HitEffect(Collision other)
    {
        ThirdPersonController.Main.LockPlayerForDuration(1f);
        Destroy(gameObject);
    }
}
