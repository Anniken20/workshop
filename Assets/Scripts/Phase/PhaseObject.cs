using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseObject : MonoBehaviour
{
    protected void OnEnable()
    {
        GhostController.onEnterPhase += OnEnter;
        GhostController.onExitPhase += OnExit;
    }

    protected void OnDisable()
    {
        GhostController.onEnterPhase -= OnEnter;
        GhostController.onExitPhase -= OnExit;
    }

    protected abstract void OnEnter();

    protected abstract void OnExit();
}
