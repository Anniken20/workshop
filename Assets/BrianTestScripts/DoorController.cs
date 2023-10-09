using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int pickup;

    private void start ()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
    }

    private void OnDoorwayClose(int pickup)
    {
        if (pickup == this.pickup){
        transform.Translate(0,-2,0);
        }
    }

    private void OnDoorwayOpen(int pickup)
    {
        if (pickup == this.pickup){
        transform.Translate(0,2,0);
        }
    }
}
