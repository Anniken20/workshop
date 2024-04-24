using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomShootableController : MonoBehaviour, IShootable
{
    public UnityEvent onShot;
    public bool multiTrigger;
    public int dmgThreshold;
    public void OnShot(BulletController bullet)
    {
            if (onShot != null)
            {
                if (bullet.currDmg < dmgThreshold)
                {
                    onShot.Invoke();
                }
                else
                {
                    return;
                }  
            }
            if (!multiTrigger) Destroy(this);
    }
}
