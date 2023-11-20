using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{
    [SerializeField] GameObject[] cubeObjs;
    private Dictionary<GameObject, Vector3> objPos = new Dictionary<GameObject, Vector3>();
    private bool returnOnDrop;
    private GameObject droppedObj;

    private void Start(){
        foreach(GameObject obj in cubeObjs){
            objPos.Add(obj, obj.transform.position);
        }
    }
    private void OnTriggerExit(Collider other){
        Debug.Log("Exiting");
        if(objPos.ContainsKey(other.gameObject)){
            if(other.gameObject.GetComponent<LassoPickupScript>().lassoedObject == other.gameObject && other.gameObject.GetComponent<LassoPickupScript>().lassoActive == true){
                droppedObj = other.gameObject;
                returnOnDrop = true;
            }
            else if(other.gameObject.GetComponent<LassoPickupScript>().lassoedObject != other.gameObject){
                var otherPos = objPos[other.gameObject];
                other.gameObject.transform.position = otherPos;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Debug.Log("Just Returning");
            }
        }
    }
    private void FixedUpdate(){
        if(returnOnDrop){
            if(droppedObj.GetComponent<LassoPickupScript>().lassoActive == false){
                Debug.Log("Dropped and returning");
                droppedObj.GetComponent<LassoPickupScript>().DropObject();
                var otherPos = objPos[droppedObj];
                droppedObj.transform.position = otherPos;
                returnOnDrop = false;
            }
        }
    }
}
