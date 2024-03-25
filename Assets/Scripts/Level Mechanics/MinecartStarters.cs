using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinecartStarters : MonoBehaviour
{
    public UnityEvent myEvent;
    public void CallEventWithDelay(float seconds)
    {
        Invoke(nameof(InvokeEvent), seconds);
    }

    private void InvokeEvent()
    {
        myEvent?.Invoke();
    }
}
