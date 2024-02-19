using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockawayPlayer : BaseTrigger
{
    public Grant grant;
    protected override void HitEffect()
    {
        grant.KnockawayPlayer();
    }
}
