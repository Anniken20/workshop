using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoScript : MonoBehaviour
{

    [SerializeField] Transform lassoAttachPoint;
    [SerializeField] float rayDistance = 50f;
    private bool lassoActive;
    private GameObject lassoObject;
    private GameObject grapplePoint;
    [SerializeField] Transform rayLaunchPoint;
    [SerializeField] Transform playerObject;
    private Rigidbody objectRigid;
    private Collider objectCollider;
    private bool moveObject;
    private bool movePlayer;
    [SerializeField] float breakDistance = 1.0f;
    [SerializeField] int flingForce;
    [SerializeField] float lassoGrabSpeed;
    private bool manipulateObject;
    void Update()
    {
        //Change this for different input this was only for testing
        if(Input.GetMouseButtonDown(0)){
        //^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
            if(!lassoActive){
                CastRay();
                manipulateObject = true;
            }
            else{
                //EmptyLasso();
                //ManipulateObject();
                //manipulateObject = true;
            }
           }
        if(Input.GetMouseButtonUp(0)){
            EmptyLasso();
            manipulateObject = false;
        }
        //Checks distance between player and grapple point and breaks off when too close
        if(grapplePoint != null){
            float distance = Vector3.Distance(playerObject.transform.position, grapplePoint.transform.position);
        
            if(distance <= breakDistance){
                movePlayer = false;
                
        }
        }

        //4 testing :)
        if(objectRigid != null){
            var objectVelocity = objectRigid.velocity;
        }
        
    }

    //Casts ray from rayLaunchPoint object and relays tag info about what the ray hit
    public void CastRay(){
        Ray ray = new Ray(rayLaunchPoint.position, rayLaunchPoint.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, rayDistance)){
            if(hit.transform.gameObject.CompareTag("Lassoable")){
                Lasso(hit.transform.gameObject);
                lassoActive = true;
            }
            /*if(hit.transform.gameObject.CompareTag("Grapple")){
                movePlayer = true;
                grapplePoint = hit.transform.gameObject;
            }*/
        }
    }

//Primes the hit game object for moving with the lasso
    public void Lasso(GameObject hitObject){
        objectRigid = hitObject.GetComponent<Rigidbody>();
        objectCollider = hitObject.GetComponent<Collider>();
        lassoObject = hitObject;
        objectRigid.useGravity = false;
        objectCollider.isTrigger = true;
        moveObject = true;
        objectRigid.velocity = Vector3.zero;
    }
//Returns object to original state and clears info
    public void EmptyLasso(){
        objectRigid.useGravity = true;
        objectCollider.isTrigger = false;
        lassoActive = false;
        moveObject = false;
        Vector3 objectVelocity = new Vector3(lassoAttachPoint.position.x*flingForce, 0, lassoAttachPoint.position.z*flingForce);
        objectRigid.velocity = objectVelocity;
        objectCollider = null;
        objectRigid = null;
    }
//Handles the movement of the lasso'ed object and player when grappling
    void FixedUpdate(){
        if(moveObject){
            lassoObject.transform.position = Vector3.Lerp(lassoObject.transform.position, lassoAttachPoint.transform.position, Time.deltaTime * lassoGrabSpeed);
        }

        if(movePlayer){
            playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, grapplePoint.transform.position, Time.deltaTime * 4);
        }

        if(manipulateObject){
            float yRotation = Input.GetAxis("Mouse Y");
            float xRotation = Input.GetAxis("Mouse X");

            lassoObject.transform.Rotate(yRotation * 10, xRotation * 10, 0);
        }
    }

/*
    private void ManipulateObject(){
        float yRotation = Input.GetAxis("MouseY");
        float xRotation = Input.GetAxis("Mouse X");

        Quaternion targetRotate = Quaternion.Euler(xRotation, yRotation, 0);
        lassoObject.transform.rotation = Quaternion.Slerp(lassoObject.transform.rotation, targetRotate, Time.deltaTime * 5.0f);
    }

*/
//Cancels the grapple if player hits something
    private void OnCollisionEnter(Collision other){
        movePlayer = false;
    }
    
}