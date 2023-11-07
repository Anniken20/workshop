using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;
    public bool multiTrigger = false;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!multiTrigger && hasTriggered)
            return;

        onTriggerEnterEvent.Invoke();
        hasTriggered = true;
    }
}
