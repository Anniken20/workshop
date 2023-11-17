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

    [HideInInspector] public bool grapple;
    private GameObject grapplePoint;
    //[SerializeField] float breakDistance = 3.0f;
    [SerializeField] float boostSpeed;
    [SerializeField] float testingBoostSpeed;
    [SerializeField] LineRenderer lineRend;
    private float checkCollisionDelay = .1f;
    private float internalCheckDelay;

    private Rigidbody rb;

    private Vector3 lassoOrigin;
    private LassoController lassoCon;

    [HideInInspector] public bool grappling;

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

    [HideInInspector] public Transform lassoConnectPoint;
    private GameObject lassoObject;

    [HideInInspector] public bool triggerGrapOnce;
    



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
            lassoObject.transform.position = grapplePoint.transform.position;
            lineRend.enabled = true;
            Vector3[] grappleConPos = new Vector3[3];
            grappleConPos[0] = lassoCon.lassoHipLocation.transform.position;
            grappleConPos[1] = lassoCon.lassoHandLocation.transform.position;

            //grappleConPos[2] = grapplePoint.transform.position;

            grappleConPos[2] = lassoConnectPoint.position;

            lineRend.positionCount = grappleConPos.Length;
            lineRend.SetPositions(grappleConPos);
            if(triggerGrapOnce){
                AutoGrapple();
                triggerGrapOnce = false;
            }
        }
    }

    private void StartGrapple(InputAction.CallbackContext context){
        EndGrapple();
        /*StopAllCoroutines();
        Debug.Log("starting");
        detectCol = false;
        //GetComponent<Rigidbody>().AddForce((grapplePoint.transform.position - transform.position) * boostSpeed, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddForce(Vector3.up * boostSpeed, ForceMode.Impulse);
        if(grapple && gPoint != null){
            grappling = true;
            //Maybe move this to fixed update idk yet
            //if(GetComponent<CharacterController>().isGrounded == false){
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
            //}
        }*/

    }

    private void AutoGrapple(){
        StopAllCoroutines();
        Debug.Log("starting");
        detectCol = false;
        //GetComponent<Rigidbody>().AddForce((grapplePoint.transform.position - transform.position) * boostSpeed, ForceMode.VelocityChange);
        var distanceBoost = Vector3.Distance(transform.position, grapplePoint.transform.position);
        GetComponent<Rigidbody>().AddForce((Vector3.up * boostSpeed) * distanceBoost, ForceMode.VelocityChange);
        if(grapple && gPoint != null){
            grappling = true;
            //Maybe move this to fixed update idk yet
            //if(GetComponent<CharacterController>().isGrounded == false){
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
            //}
        }
    }

    private void EndGrapplePressed(InputAction.CallbackContext context){
        //EndGrapple();

    }

    private void EndGrapple(){
        grappling = false;
        if(lassoObject != null){
            lassoObject.GetComponent<LassoDetection>().recall = true;
        }
        lassoCon.endThrow = true;
        Debug.Log("Ending");
        //grappling = false;
        grapple = false;
        if(gJoint != null){
            Destroy(gJoint);
        }
        lineRend.enabled = false;
        detectCol = true;
        StartCoroutine(EndDelay());
        StartCoroutine(CharDelay());
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
        lassoObject = GetComponent<LassoController>().projectile;
        lassoConnectPoint = GetComponent<LassoController>().connectPoint;
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
        yield return new WaitForSeconds(0.2f);
        canLasso = true;
    }
    private IEnumerator CharDelay(){
        yield return new WaitForSeconds(1.5f);
        EnableChar();
    }

    private void OnCollisionEnter(Collision other){
        if(detectCol){
            EnableChar();
        }
    }

    private void EnableChar(){
        GetComponent<ThirdPersonController>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


}