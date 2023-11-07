using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCombat : MonoBehaviour
{
    private int toggle = 0;
    private GameObject otherObj;
    private bool inCombat;
    private void OnTriggerEnter(Collider other){
        otherObj = other.gameObject;
        if(other.gameObject.tag == "Player" && toggle == 0 && !inCombat){
            otherObj.gameObject.GetComponent<LassoController>().inCombat = true;
            toggle = 1;

        }
        else if(other.gameObject.tag == "Player" && toggle == 0 && inCombat){
            otherObj.gameObject.GetComponent<LassoController>().inCombat = false;
            toggle = 1;
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            toggle = 0;
        }

    }

    private void Update(){
        if(otherObj != null){
            inCombat = otherObj.gameObject.GetComponent<LassoController>().inCombat;

        }
    }
}
