using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LassoPickupScript : MonoBehaviour, ILassoable
{

    public CharacterMovement iaControls;
    private InputAction lasso;
    private InputAction look;
    private InputAction push;
    private InputAction pull;
    private InputAction manipulate;

    private Vector3 launchAngle;
    private Rigidbody rb;
    private Collider objectCollider;

    private bool moveObject;

    private Transform attachPoint;

    [HideInInspector] public bool manipulateObject;

    [SerializeField] float objectWeight;

    [SerializeField] float launchForce;

    private bool lassoActive;

    private LassoController player;

    private Transform attachOrigin;

    private Vector3 objectPos;

    private Vector3 playerForward;

    private GameObject lassoedObject;


    private float throwWindow = 3f;
    private float internalThrowWindow;
    private bool throwEnabled;
    private bool canScroll;

    private bool inCombat;
    private float combatLaunchStrength;
    Camera mainCam;

    private Vector3 throwPoint;
    private float lassoCooldown;
    private bool throwing;
    private GameObject lassoObject;
    private float mWheelDistance;
    private bool pulling;
    private bool pushing;
    




    private void Start(){
        mainCam = Camera.main;
        internalThrowWindow = throwWindow;
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        player = FindObjectOfType<LassoController>();
        lassoCooldown = player.lassoCooldown;
        //combatLaunchStrength = player.GetComponent<LassoController>().combatLaunchStrength;


    }

    private void Update(){

            if(Input.GetAxis("Mouse ScrollWheel") != 0){
                mWheelDistance = Input.GetAxis("Mouse ScrollWheel");
            }

        iaControls.CharacterControls.Push.performed += OnPushStart;
        iaControls.CharacterControls.Push.canceled += OnPushEnd;

        iaControls.CharacterControls.Pull.performed += OnPullStart;
        iaControls.CharacterControls.Pull.canceled += OnPullEnd;

        lassoObject = player.GetComponent<LassoController>().projectile;
        //Debug.Log(internalThrowWindow);
        inCombat = player.GetComponent<LassoController>().inCombat;
        if(inCombat && internalThrowWindow > 0 && lasso.triggered && lassoedObject != null){
            LaunchToCursor();
        }
        //Debug.Log(internalThrowWindow);
        if(inCombat && internalThrowWindow <= 0){
            LaunchToCursor();
            throwEnabled = false;

        }

        if(throwEnabled){
            internalThrowWindow -= Time.deltaTime;
        }
        else{
            internalThrowWindow = throwWindow;
        }

        launchAngle = player.GetComponent<AimController>().GetAimAngle();
        if(lasso.triggered && lassoActive == true){
            DropObject();
        }

        /*if(Input.GetKeyDown(KeyCode.F) && !inCombat){
            manipulateObject = true;
            playerForward = player.transform.forward;
            //attachOrigin = attachPoint;
        }
        else if(Input.GetKeyUp(KeyCode.F) && !inCombat){
            manipulateObject = false;
            //attachPoint.transform.position = attachOrigin.transform.position;
        }*/

        iaControls.CharacterControls.Manipulate.started += OnManipulateStart;
        iaControls.CharacterControls.Manipulate.canceled += OnManipulateEnd;

        player.holdingItem = lassoActive;
    }
    public void Lassoed(Transform lassoAttachPoint, bool active, GameObject otherObject){
        if(!inCombat){
            lassoActive = active;
            lassoedObject = otherObject;
            attachPoint = lassoAttachPoint;
            rb.useGravity = false;
            objectCollider.isTrigger = true;
            moveObject = active;
            rb.velocity = Vector3.zero;
            //manipulateObject = true;
            player.startLassoCooldown = false;
            mWheelDistance = 0;
        }
        else{
            CombatLassoEnabled();
            lassoedObject = otherObject;
        }
    }

    private void CombatLassoEnabled(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        throwEnabled = true;
    }
    private void LaunchToCursor(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
            if(lassoedObject != null){
                throwPoint = hit.point;
                throwPoint.y += 2.5f;
            }
        }
           // combatLaunchStrength = combatLaunchStrength * objectWeight;
           combatLaunchStrength = 1.5f;
           var yBoost = lassoedObject.transform.position;
           //yBoost.y += .5f;
            //lassoedObject.transform.position = yBoost;
            lassoedObject.GetComponent<Rigidbody>().AddForce((throwPoint - lassoedObject.transform.position) * combatLaunchStrength, ForceMode.VelocityChange);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            throwEnabled = false;
            lassoedObject = null;

            player.internalCooldown = lassoCooldown;
            player.startLassoCooldown = true;
            throwing = true;
    }
    private void DropObject(){
        player.startLassoCooldown = true;
        rb.useGravity = true;
        objectCollider.isTrigger = false;
        moveObject = false;
        //rb.velocity = new Vector3(attachPoint.position.x * launchForce, 0, attachPoint.position.y * launchForce);
        rb.AddForce(launchAngle * launchForce, ForceMode.Impulse);
        //manipulateObject = false;
        lassoActive = false;
        lassoedObject = null;
        player.drawToLasso = false;
        player.drawToLassoLine.enabled = false;
        lassoObject.GetComponent<LassoDetection>().destroy = true;
    }

    private void FixedUpdate(){
        if(moveObject && manipulateObject == false){
            transform.position = Vector3.Lerp(transform.position, attachPoint.transform.position, Time.deltaTime * objectWeight);
        }

        if(manipulateObject && lassoedObject != null){
            //This will need to be reworked for controller support
            var looking = look.ReadValue<Vector2>();
            float yRotation = looking.y;
            float xRotation = looking.x;
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            lassoedObject.transform.Rotate(yRotation * objectWeight, xRotation * objectWeight, 0);
            if(pushing || pulling){
                if(pushing){
                    mWheelDistance += .1f;
                }
                if(pulling){
                    mWheelDistance -= .1f;
                }
                var newPos = transform.position + (playerForward * mWheelDistance * 20);
                var objDistance = Vector3.Distance(lassoedObject.transform.position, player.transform.position);
                lassoedObject.transform.position = Vector3.Lerp(lassoedObject.transform.position, newPos, Time.deltaTime);
                if(objDistance >= 5 || objDistance <= 2.1){
                DropObject();
            }
            else{
                newPos = lassoedObject.transform.position;
                mWheelDistance = 0f;
            }
                //Debug.Log(objDistance);
            }
            //newPos = Mathf.Clamp(newPos, attachPoint.position, maxDistance);
        
            }

        }

    void OnCollisionEnter(Collision other){
        if(throwing){
            lassoObject.GetComponent<LassoDetection>().destroy = true;
            player.GetComponent<LassoController>().drawToLasso = false;
            throwing = false;
        }
    }

    private void Awake(){
        iaControls = new CharacterMovement();
    }

    private void OnEnable(){
        look = iaControls.CharacterControls.Look;
        lasso = iaControls.CharacterControls.Lasso;
        push = iaControls.CharacterControls.Push;
        pull = iaControls.CharacterControls.Pull;
        manipulate  =iaControls.CharacterControls.Manipulate;

        manipulate.Enable();
        push.Enable();
        pull.Enable();
        look.Enable();
        lasso.Enable();
    }
    private void OnDisable(){
        manipulate.Disable();
        push.Disable();
        pull.Disable();
        look.Disable();
        lasso.Disable();
    }

    private void OnManipulateStart(InputAction.CallbackContext context){
            manipulateObject = true;
            playerForward = player.transform.forward;
            //attachOrigin = attachPoint;
    }
    private void OnManipulateEnd(InputAction.CallbackContext context){
            manipulateObject = false;
            //attachPoint.transform.position = attachOrigin.transform.position;
    }

    private void OnPullStart(InputAction.CallbackContext context){
        pulling = true;
    }
    private void OnPullEnd(InputAction.CallbackContext context){
        pulling = false;
    }
    private void OnPushStart(InputAction.CallbackContext context){
        pushing = true;
    }
    private void OnPushEnd(InputAction.CallbackContext context){
        pushing = false;
    }

}
