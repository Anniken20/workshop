using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoPickupScript : MonoBehaviour, ILassoable
{
    private Rigidbody rb;
    private Collider objectCollider;

    private bool moveObject;

     private Transform attachPoint;

    private bool manipulateObject;

    [SerializeField] float objectWeight;

    [SerializeField] float launchForce;


    private void Start(){
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    private void Update(){
        if(Input.GetMouseButtonUp(1)){
            DropObject();
        }
    }
    public void Lassoed(Transform lassoAttachPoint, bool active){
        attachPoint = lassoAttachPoint;
        rb.useGravity = false;
        objectCollider.isTrigger = true;
        moveObject = active;
        rb.velocity = Vector3.zero;
        manipulateObject = true;

    }

    private void DropObject(){
        rb.useGravity = true;
        objectCollider.isTrigger = false;
        moveObject = false;
        rb.velocity = new Vector3(attachPoint.position.x * launchForce, 0, attachPoint.position.y * launchForce);
        manipulateObject = false;
    }

    private void FixedUpdate(){
        if(moveObject){
            transform.position = Vector3.Lerp(transform.position, attachPoint.transform.position, Time.deltaTime * objectWeight);
        }

        if(manipulateObject){
            //This will need to be reworked for controller support
            float yRotation = Input.GetAxis("Mouse Y");
            float xRotation = Input.GetAxis("Mouse X");
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            transform.Rotate(yRotation * objectWeight, xRotation * objectWeight, 0);
        }
    }
}
