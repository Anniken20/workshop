using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventTrigger : BaseTrigger
{
    public UnityEvent playerHit;
    protected override void HitEffect()
    {
        playerHit.Invoke();
    }
}
