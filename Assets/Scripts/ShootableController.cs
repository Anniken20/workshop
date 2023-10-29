using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootableController : MonoBehaviour
{
    public UnityEvent onShot;
    public GameObject gate;

    public void OnShot()
    {
        if (onShot != null)
        {
            onShot.Invoke();
        }
    }
}
