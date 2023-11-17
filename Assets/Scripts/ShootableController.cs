using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootableController : MonoBehaviour
{
    public UnityEvent onShot;
    public bool multiTrigger;

    public void OnShot()
    {
        if (onShot != null)
        {
            onShot.Invoke();
        }
        if(!multiTrigger) Destroy(this);
    }
}
