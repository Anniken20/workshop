using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class LassoGrappleScript : MonoBehaviour, IGrappleable
{
    public CharacterMovement iaControls;
    private InputAction lasso;
    private InputAction cancel;

    private bool grapple;
    private GameObject grapplePoint;
    //[SerializeField] float breakDistance = 3.0f;
    [SerializeField] float boostSpeed;
    [SerializeField] LineRenderer lineRend;
    private float checkCollisionDelay = .1f;
    private float internalCheckDelay;

    private Rigidbody rb;

    private Vector3 lassoOrigin;
    private LassoController lassoCon;

    private bool grappling;

    public bool canLasso = true;

    private Vector3 gPoint;
    private SpringJoint gJoint;

    public bool detectCol;

    [SerializeField] float maxDist;
    [SerializeField] float minDist;
    [SerializeField] float sPower;
    [SerializeField] float dPower;
    [SerializeField] float mScale;

    [SerializeField] float cancelDistance;
    private float walkSpeed;
    



    public void Grappled(bool active, GameObject hitObject){
        canLasso = false;
        grappling = true;
        grapplePoint = hitObject;
        grapple = active;

    }

    void Start(){
        
        lassoCon = this.GetComponent<LassoController>();
        rb = GetComponent<Rigidbody>();
        lineRend.enabled = false;
    }
    private void Awake(){
        iaControls = new CharacterMovement();
    }

    void FixedUpdate(){
        if(grapple){
            //StopAllCoroutines();
            /*rb.AddForce((grapplePoint.transform.position - transform.position) * grappleSpeed, ForceMode.VelocityChange);
            GetComponent<CharacterController>().enabled = true;
            lineRend.enabled = true;
            StartCoroutine(HookLifetime());*/
        }


            ///Added///

        lassoOrigin = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);


        if(grapple){
            lineRend.enabled = true;
            Vector3[] grappleConPos = new Vector3[3];
            grappleConPos[0] = lassoCon.lassoHipLocation.transform.position;
            grappleConPos[1] = lassoCon.lassoHandLocation.transform.position;
            //grappleConPos[1] = lassoOrigin;
            grappleConPos[2] = grapplePoint.transform.position;
            lineRend.positionCount = grappleConPos.Length;
            lineRend.SetPositions(grappleConPos);
            //lineRend.SetPosition(0, lassoOrigin);
            //lineRend.SetPosition(1, grapplePoint.transform.position);
        }
    }

    private void StartGrapple(InputAction.CallbackContext context){
        Debug.Log("starting");
        detectCol = false;
        GetComponent<Rigidbody>().AddForce((grapplePoint.transform.position - transform.position) * boostSpeed, ForceMode.VelocityChange);
        if(grapple && gPoint != null){
            grappling = true;
            //Maybe move this to fixed update idk yet
            if(GetComponent<CharacterController>().isGrounded == false){
                GetComponent<CharacterController>().enabled = false;
                GetComponent<ThirdPersonController>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                gPoint = grapplePoint.transform.position;
                if(GetComponent<SpringJoint>() == null){
                    gJoint = gameObject.AddComponent<SpringJoint>();
                }
                if(gJoint != null){
                    gJoint.autoConfigureConnectedAnchor = false;
                    gJoint.connectedAnchor = gPoint;

                    float dist = Vector3.Distance(gameObject.transform.position, gPoint);
                    gJoint.maxDistance = dist * maxDist;
                    gJoint.minDistance = dist * minDist;

                    //gJoint.maxDistance = maxDist;
                    //gJoint.minDistance = minDist;


                    gJoint.spring = sPower;
                    gJoint.damper = dPower;
                    gJoint.massScale = mScale;

                    
                }
            }
        }

    }

    private void EndGrapplePressed(InputAction.CallbackContext context){
        EndGrapple();

    }

    private void EndGrapple(){
        Debug.Log("Ending");
        grappling = false;
        grapple = false;
        if(gJoint != null){
            Destroy(gJoint);
        }
        lineRend.enabled = false;
        detectCol = true;
        StartCoroutine(EndDelay());
    }
    private void OnEnable(){
        lasso = iaControls.CharacterControls.Lasso;
        cancel = iaControls.CharacterControls.CancelAim;

        cancel.Enable();
        lasso.Enable();

    }
    private void OnDisable(){
        cancel.Disable();
        lasso.Disable();
    }

    private void Update(){
        //Debug.Log(Vector3.Distance(gameObject.transform.position, gPoint));
        if(grapple){
            iaControls.CharacterControls.Lasso.started += StartGrapple;
            iaControls.CharacterControls.Lasso.canceled += EndGrapplePressed;
        }
        if(grapple || grappling){
            canLasso = false;
        }
        if(cancel.triggered){
            EndGrapple();
        }
        if(grapplePoint != null){
            if(Vector3.Distance(gameObject.transform.position, grapplePoint.transform.position) > cancelDistance && grapple){
                Debug.Log(Vector3.Distance(gameObject.transform.position, grapplePoint.transform.position));
                EndGrapple();
            }
        }
        



    }

    private IEnumerator EndDelay(){
        yield return new WaitForSeconds(0.5f);
        canLasso = true;
    }

    private void OnCollisionEnter(Collision other){
        if(detectCol){
            GetComponent<ThirdPersonController>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }


}



















/*
    private IEnumerator HookLifetime(){
        GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(8f);
        lineRend.enabled = false;
        grapple = false;
        rb.velocity = Vector3.zero;
        GetComponent<CharacterController>().enabled = true;
    }

    private void Update(){
        if(grapplePoint != null){
            float distance = Vector3.Distance(transform.position, grapplePoint.transform.position);
            //Debug.Log(distance);
            if(distance <= breakDistance){
                //Debug.Log("Breaking off");
                grapple = false;
                lineRend.enabled = false;
                rb.velocity = Vector3.zero;
                GetComponent<CharacterController>().enabled = true;
            }
        }
        if(grapple){
            //lineRend.enabled = true;
            //lineRend.SetPosition(0, lassoOrigin);
            //lineRend.SetPosition(1, grapplePoint.transform.position);
            internalCheckDelay -= Time.deltaTime;
        }
        else{
            internalCheckDelay = checkCollisionDelay;
        }

        if(Input.GetMouseButtonUp(1)){
            grapple = false;
            GetComponent<CharacterController>().enabled = true;
            rb.velocity = Vector3.zero;
            lineRend.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other){
        if(internalCheckDelay <= 0){
            grapple = false;
            lineRend.enabled = false;
            GetComponent<CharacterController>().enabled = true;
        }
    }
}*/

