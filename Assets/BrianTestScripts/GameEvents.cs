using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> onDoorwayTriggerEnter;
    public void DoorwayTriggerEnter(int pickup)
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(pickup);
        }
    }

    public event Action<int> onDoorwayTriggerExit;
    public void DoorwayTriggerExit(int pickup)
    {
        if(onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(pickup);
        }
    }
}
