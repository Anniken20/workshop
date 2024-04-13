using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToDeathLocation : MonoBehaviour
{
    [SerializeField] Transform deathLocation;
    public void TPtoDeath(){
        Debug.Log("TELEPORTING");
        this.transform.position = deathLocation.position;
    }
}
