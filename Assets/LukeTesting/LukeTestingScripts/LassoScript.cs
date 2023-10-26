using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoScript : MonoBehaviour
{
[Header("Objects and transform info")]

    [SerializeField] Transform rayLaunchPoint;
    [SerializeField] Transform lassoAttachPoint;
    
    [SerializeField] Transform playerObject;
[Header("Lasso Stats")]
    [SerializeField] float rayDistance = 50f;
    [SerializeField] int flingForce;
    [SerializeField] float lassoGrabSpeed;
[Header("Grapple Stats")]
    private bool lassoActive;
    private GameObject lassoObject;
    private GameObject grapplePoint;
    private Rigidbody objectRigid;
    private Collider objectCollider;
    private bool moveObject;
    private bool grapple;
    [SerializeField] float breakDistance = 1.0f;

    [SerializeField] float grappleSpeed;
    private bool manipulateObject;
    private Rigidbody rb;
[Header("Misc")]
    [SerializeField] LineRenderer lineRend;

    private void Start(){
        //get player's rigidbody in case this is setup on a child object
        rb = playerObject.gameObject.GetComponent<Rigidbody>();
        lineRend.enabled = false;
    }
    void Update()
    {
        //Change this for different input this was only for testing
        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.G)){
        //^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^ ^
            if(!lassoActive){
                CastRay();
                manipulateObject = true;
            }
           }
        if(Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.G))
        {
            EmptyLasso();
            manipulateObject = false;
            grapple = false;
            lineRend.enabled = false;
            rb.velocity = Vector3.zero;
            playerObject.gameObject.GetComponent<CharacterController>().enabled = true;
        }
        //Checks distance between player and grapple point and breaks off when too close
        if(grapplePoint != null){
            float distance = Vector3.Distance(playerObject.transform.position, grapplePoint.transform.position);
        
            if(distance <= breakDistance){
                grapple = false;
                lineRend.enabled = false;
                playerObject.gameObject.GetComponent<CharacterController>().enabled = true;

        
                
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
            if(hit.transform.gameObject.CompareTag("Grapple")){
                grapple = true;
                grapplePoint = hit.transform.gameObject;
                lineRend.enabled = true;
                rb.velocity = Vector3.zero;
            }
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
        if(objectRigid || objectCollider != null){
            objectRigid.useGravity = true;
            objectCollider.isTrigger = false;
            lassoActive = false;
            moveObject = false;
            Vector3 objectVelocity = new Vector3(lassoAttachPoint.position.x*flingForce, 0, lassoAttachPoint.position.z*flingForce);
            objectRigid.velocity = objectVelocity;
            objectCollider = null;
            objectRigid = null;
        }
    }
//Handles the movement of the lasso'ed object and player when grappling
    void FixedUpdate(){
        if(moveObject){
            
            lassoObject.transform.position = Vector3.Lerp(lassoObject.transform.position, lassoAttachPoint.transform.position, Time.deltaTime * lassoGrabSpeed);
        }
//Moves the player towards the grapple point
        if(grapple){
            StopAllCoroutines();
            //playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, grapplePoint.transform.position, Time.deltaTime * grappleSpeed);
            rb.AddForce((grapplePoint.transform.position - playerObject.transform.position) * .1f, ForceMode.VelocityChange);
            playerObject.gameObject.GetComponent<CharacterController>().enabled = true;
            lineRend.SetPosition(0, rayLaunchPoint.transform.position);
            lineRend.SetPosition(1, grapplePoint.transform.position);
            StartCoroutine(HookLifetime());
        }

        if(manipulateObject){
            //This will need to be reworked for controller support
            float yRotation = Input.GetAxis("Mouse Y");
            float xRotation = Input.GetAxis("Mouse X");
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            if(lassoObject != null){
                lassoObject.transform.Rotate(yRotation * 10, xRotation * 10, 0);
            }
        }

    }


//Cancels the grapple if player hits something and after a certain amount of time
    private void OnCollisionEnter(Collision other){
        grapple = false;
        lineRend.enabled = false;
        playerObject.gameObject.GetComponent<CharacterController>().enabled = true;
    }
//Cancels the grapple if it is going for too long
    private IEnumerator HookLifetime(){
        playerObject.gameObject.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(8f);
        lineRend.enabled = false;
        grapple = false;
        playerObject.gameObject.GetComponent<CharacterController>().enabled = true;
        
    }
    
}