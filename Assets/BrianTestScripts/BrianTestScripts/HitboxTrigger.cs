using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;
    public UnityEvent onTriggerExitEvent;
    public bool multiTrigger = false;
    [Tooltip("Only triggers if the player enters/exits")]
    public bool playerOnly = true;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!multiTrigger && hasTriggered)
            return;

        if (!playerOnly || other.gameObject.CompareTag("Player"))
        {
            onTriggerEnterEvent.Invoke();
            hasTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerOnly || other.gameObject.CompareTag("Player"))
        {
            onTriggerExitEvent.Invoke();
        }
    }
}
