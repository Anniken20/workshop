using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhaseObject : MonoBehaviour
{
    protected void Start()
    {
        GhostController.onEnterPhase += OnEnter;
        GhostController.onExitPhase += OnExit;
    }

    protected abstract void OnEnter();

    protected abstract void OnExit();
}
