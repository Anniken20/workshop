using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject toPoint;
    public bool tpEnabled;

    private void Update(){
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && tpEnabled)
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            other.gameObject.transform.position = toPoint.transform.position;
            other.gameObject.GetComponent<CharacterController>().enabled = true;
            tpEnabled = false;

        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            tpEnabled = true;
            toPoint.GetComponent<TeleportController>().tpEnabled = false;
        }
    }
}