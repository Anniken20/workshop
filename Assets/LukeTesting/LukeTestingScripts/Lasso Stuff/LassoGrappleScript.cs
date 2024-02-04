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
    [SerializeField] float boostSpeed = 0.75f;
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
    public bool autoRelease = false;
    



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


            grappleConPos[2] = lassoConnectPoint.position;

            lineRend.positionCount = grappleConPos.Length;
            lineRend.SetPositions(grappleConPos);
            if(triggerGrapOnce){
                AutoGrapple();
                triggerGrapOnce = false;
            }
        }

        if(grapplePoint != null){
            if(autoRelease == true){
                if(this.transform.position.y - grapplePoint.transform.position.y >= -2.8){
                    EndGrapple();
                }
            }
        }
        
    }

    private void StartGrapple(InputAction.CallbackContext context){
        EndGrapple();
    }

    private void AutoGrapple(){
        StopAllCoroutines();
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        detectCol = false;
        var distanceBoost = Vector3.Distance(transform.position, grapplePoint.transform.position);
        GetComponent<Rigidbody>().AddForce((Vector3.up * boostSpeed) * distanceBoost, ForceMode.VelocityChange);
        if(grapple && gPoint != null){
            grappling = true;
            StartCoroutine(ReleaseDelay());
                //GetComponent<CharacterController>().enabled = false;
                GetComponent<ThirdPersonController>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
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

                    gJoint.spring = sPower;
                    gJoint.damper = dPower;
                    gJoint.massScale = mScale;

                    
                }
        }
    }

    private void EndGrapplePressed(InputAction.CallbackContext context){
        //EndGrapple();

    }

    private void EndGrapple(){
        if(grappling){
            if(lassoObject != null){
                lassoObject.GetComponent<LassoDetection>().recall = true;
            }
            grappling = false;
            lassoCon.endThrow = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            grapple = false;
            if(gJoint != null){
                Destroy(gJoint);
            }
            lineRend.enabled = false;
            detectCol = true;
            StartCoroutine(EndDelay());
            StartCoroutine(CharDelay());
            autoRelease = false;
        }
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
            /*if(Vector3.Distance(gameObject.transform.position, grapplePoint.transform.position) > cancelDistance && grapple){;
                EndGrapple();
            }*/
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
    private IEnumerator ReleaseDelay(){
        yield return new WaitForSeconds(1f);
        autoRelease = true;
    }

    private void EnableChar(){
        GetComponent<ThirdPersonController>().enabled = true;
        //GetComponent<CharacterController>().enabled = true;
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = true;
    }


}