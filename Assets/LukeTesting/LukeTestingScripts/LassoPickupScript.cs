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

    private bool lassoActive;

    private LassoController player;

    private Transform attachOrigin;

    private Vector3 objectPos;

    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    private Vector3 playerForward;

    [HideInInspector] public GameObject lassoedObject;


    private void Start(){
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        player = FindObjectOfType<LassoController>();


    }

    private void Update(){
        
        if(Input.GetMouseButtonDown(1) && lassoActive == true){
            DropObject();
        }

        if(Input.GetKeyDown(KeyCode.F)){
            manipulateObject = true;
            playerForward = player.transform.forward;
            //attachOrigin = attachPoint;
        }
        else if(Input.GetKeyUp(KeyCode.F)){
            manipulateObject = false;
            //attachPoint.transform.position = attachOrigin.transform.position;
        }

        player.holdingItem = lassoActive;
    }
    public void Lassoed(Transform lassoAttachPoint, bool active){
        lassoActive = active;
        attachPoint = lassoAttachPoint;
        rb.useGravity = false;
        objectCollider.isTrigger = true;
        moveObject = active;
        rb.velocity = Vector3.zero;
        //manipulateObject = true;
        player.startLassoCooldown = false;

    }

    private void DropObject(){
        player.startLassoCooldown = true;
        rb.useGravity = true;
        objectCollider.isTrigger = false;
        moveObject = false;
        rb.velocity = new Vector3(attachPoint.position.x * launchForce, 0, attachPoint.position.y * launchForce);
        //manipulateObject = false;
        lassoActive = false;
        lassoedObject = null;
    }

    private void FixedUpdate(){
        if(moveObject && manipulateObject == false){
            transform.position = Vector3.Lerp(transform.position, attachPoint.transform.position, Time.deltaTime * objectWeight);
        }

        if(manipulateObject && lassoedObject != null){
            //This will need to be reworked for controller support
            float yRotation = Input.GetAxis("Mouse Y");
            float xRotation = Input.GetAxis("Mouse X");
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            lassoedObject.transform.Rotate(yRotation * objectWeight, xRotation * objectWeight, 0);
            float mWheelDistance = Input.mouseScrollDelta.y;
            //transform.position = Mathf.Clamp(, minDistance, maxDistance);
            var newPos = transform.position + (playerForward * mWheelDistance * 20);
            lassoedObject.transform.position = Vector3.Lerp(lassoedObject.transform.position, newPos, Time.deltaTime);
        
    }
}
}
