using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LassoController : MonoBehaviour

{
    public CharacterMovement iaControls;
    private InputAction lasso;
    private InputAction look;
    private InputAction cancel;
    public AimController aimController;
    private Vector3 aimAngle;
    private Vector3 startPos;
    private Vector3 lineStart;
    private bool lassoActive;
    [SerializeField] public Transform lassoAttachPoint;
    //[SerializeField] Vector3 lineOrigin;
    //[SerializeField] float rayDistance;

    [Header("For Lasso Specific Aiming")]
    [SerializeField] Transform launchPoint;
    [SerializeField] LineRenderer lineRend;
    //[SerializeField] Transform releasePos;
    //[SerializeField] [Range(10,100)] private int linePoints = 25;
    private int linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float tBetween = 0.1f;
    [SerializeField] GameObject lassoObject;
    [SerializeField] [Range(1f, 15f)] private float lassoLaunchStrength;
    [Range(0.5f, 5f)] public float lassoCooldown;
    [HideInInspector] public float internalCooldown;
    [HideInInspector] public bool startLassoCooldown = true;
    [HideInInspector] public bool holdingItem;
    [HideInInspector] public bool drawToLasso;
    [SerializeField] public LineRenderer drawToLassoLine;
    [HideInInspector] public GameObject projectile;
    [SerializeField] GameObject lassoSpinLocation;
    [SerializeField] public GameObject lassoHandLocation;
    [SerializeField] public GameObject lassoHipLocation;
    private Transform spinningConnectPoint;
    [HideInInspector] public GameObject spinningLasso;
    private bool spinning = false;
    private LayerMask lassoAimMask;
    public bool inCombat;
    [HideInInspector] public Transform connectPoint;
    private float mouseY;
    private float mouseX;
    private Vector3 yAdjusted;
    private float sens;
    private float yAngleFreedom = 1f;
    private string lassoState;
    private Vector2 looking;
    
    [SerializeField] private AudioClip spinSound;
    public AudioClip thrownSound;
    [SerializeField] private bool cancelAim;
    public GameObject lassoCombatAiming;

    [Header("Lasso Spin Speed")]
    [SerializeField] [Range(0f, 30f)] private float spinSpeed = 4.5f;

    private Animator animator;

    [HideInInspector] public bool endThrow = true;


    private void Awake(){
        internalCooldown = lassoCooldown;
        int lassoLayer = lassoObject.gameObject.layer;
        for(int i = 0; i<32; i++){
            if(!Physics.GetIgnoreLayerCollision(lassoLayer, i)){
                lassoAimMask |= 1 << i;
            }
        }
        iaControls = new CharacterMovement();
    }
    void Update(){
        if(endThrow){
            animator.SetBool("isThrowing", false);
            animator.SetBool("isLassoing", false);
            endThrow = false;
            
        }
        if(cancel.triggered){
            CancelAiming();
        }
        if(spinningLasso != null){
            spinningLasso.transform.Rotate(Vector3.up, spinSpeed);
            animator.SetBool("isLassoing", true);
        }
        
        looking = look.ReadValue<Vector2>();

        //iaControls.CharacterControls.Lasso.started += OnLassoDown;
        //iaControls.CharacterControls.Lasso.canceled += OnLassoRelease;



        /*if(drawToLasso){
            if(projectile != null){
                connectPoint = projectile.transform.Find("ConnectPoint");
                
                /*lineStart = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y + 1.5f,
                gameObject.transform.position.z)
                + (gameObject.transform.forward * 0.25f);////
                drawToLassoLine.enabled = true;
                drawToLassoLine.SetPosition(0, lassoHipLocation.transform.position);
                drawToLassoLine.SetPosition(1, connectPoint.position);
            }
        }
        else if(drawToLasso == false){
            drawToLassoLine.enabled = false;
        }*/
        if(startLassoCooldown){
            internalCooldown -= 1f * Time.deltaTime;
        }
        if(internalCooldown <= 0){
            internalCooldown = 0;
        }
       /* if(Input.GetMouseButtonDown(1)){
            if(holdingItem == false){
                lassoActive = true;
                
            }
        }
        if(Input.GetMouseButtonUp(1)){
            lassoActive = false;
            if(holdingItem == false && internalCooldown <= 0){
                LaunchLasso();
                internalCooldown = lassoCooldown;
                lineRend.enabled = false;
            }
        }*/

        



        /*if(lassoActive && holdingItem == false){
            startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
            if(internalCooldown <= 0){
                DrawLassoLine();
            }
        }
        LassoAiming();*/
    }


    private void DrawLassoLine(){
        drawToLasso = true;
        if(spinning && lassoActive && holdingItem == false){
            spinningLasso = Instantiate(lassoObject, lassoSpinLocation.transform);
            AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, spinSound);
            spinning = false;
        }
        //spinningLasso = Instantiate(lassoObject, lassoSpinLocation.transform);
        if(spinningLasso != null){
            spinningConnectPoint = spinningLasso.transform.Find("ConnectPoint");
            drawToLassoLine.enabled = true;
            spinningLasso.GetComponent<Rigidbody>().isKinematic = true;
        
        
        //drawToLassoLine.SetPosition(0, lassoHipLocation.transform.position);
        //drawToLassoLine.SetPosition(1, spinningConnectPoint.position);
        




        lineRend.enabled = true;
        lineRend.positionCount = Mathf.CeilToInt(linePoints / tBetween) + 1;

        Vector3 lassoVelocity = lassoLaunchStrength * aimAngle / lassoObject.GetComponent<Rigidbody>().mass;

        int i = 0;

        lineRend.SetPosition(i, lassoSpinLocation.transform.position);
        for(float time = 0; time < linePoints; time += tBetween){
            i++;
            Vector3 point  = lassoSpinLocation.transform.position + time * lassoVelocity;
            point.y = lassoSpinLocation.transform.position.y + lassoVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRend.SetPosition(i, point);

            Vector3 lastPosition = lineRend.GetPosition(i - 1);
            if(Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, lassoAimMask)){
                lineRend.SetPosition(i, hit.point);
                lineRend.positionCount = i + 1;
                return;
            }
        }
        }
    }

    private void LaunchLasso(){
        animator.SetBool("isLassoing", false);
        endThrow = false;
        animator.SetBool("isThrowing", true);
        //lassoSpinLocation.transform.Rotate(new Vector3(0f, 0f, 0f));
        AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, thrownSound);
        spinning = false;
        Destroy(spinningLasso);
        projectile = Instantiate(lassoObject, lassoSpinLocation.transform.position, launchPoint.rotation);
        projectile.transform.Rotate(0f, aimAngle.y, 0f);
        //aimAngle = aimController.GetAimAngle();
        projectile.GetComponent<Rigidbody>().AddForce(aimAngle * lassoLaunchStrength, ForceMode.Impulse);
        drawToLasso = true;

    }

    private void Start(){
        animator = GetComponent<Animator>();
        sens = aimController.sensitivity;
        drawToLasso = true;
        lineRend.enabled = true;
        drawToLassoLine.enabled = true;

        yAdjusted = new Vector3();
        aimAngle = new Vector3();
        //iActions = new CharacterControls();
    }

    private void LassoAiming(){

        mouseY = looking.y;
        //mouseX = looking.x;

        mouseY *= sens / 100;
        //mouseX *= sens / 2;

        yAdjusted.y += mouseY;

        float yClamp = Mathf.Clamp(yAdjusted.y, -yAngleFreedom / 2, yAngleFreedom / 2);

        yAdjusted.y = yClamp;
        //yAdjusted.x = mouseX;
        //lassoSpinLocation.transform.Rotate(new Vector3(0f, mouseX, 0f));
        //aimAngle = lassoSpinLocation.transform.forward;
        aimAngle = aimController.GetAimAngle();
        //Debug.Log(aimAngle);
        aimAngle *= 0.2f;
        //aimAngle = gameObject.transform.forward;
        aimAngle += yAdjusted;

        
    }

    private void OnEnable(){
        look = iaControls.CharacterControls.Look;
        lasso = iaControls.CharacterControls.Lasso;
        cancel = iaControls.CharacterControls.CancelAim;

        look.Enable();
        lasso.Enable();
        cancel.Enable();
    }
    private void OnDisable(){
        look.Disable();
        lasso.Disable();
        cancel.Disable();
    }

    private void OnLassoDown(InputAction.CallbackContext context){
        if(holdingItem == false && GetComponent<LassoGrappleScript>().canLasso == true){
            lassoActive = true;  
            spinning = true;
            cancelAim = false;
        }
    }
    private void OnLassoRelease(InputAction.CallbackContext context){
        lassoActive = false;
        if(holdingItem == false && internalCooldown <= 0 && cancelAim == false && GetComponent<LassoGrappleScript>().canLasso == true){
            LaunchLasso();
            internalCooldown = lassoCooldown;
            lineRend.enabled = false;
            cancelAim = false;
        }
        if(spinningLasso != null){
            foreach (Transform child in lassoSpinLocation.transform){
                Destroy(child.gameObject);
            }
        }
    }
    private void FixedUpdate(){

        iaControls.CharacterControls.Lasso.started += OnLassoDown;
        iaControls.CharacterControls.Lasso.canceled += OnLassoRelease;
        
        /*if(inCombat){
            lassoCombatAiming.SetActive(true);
        }*/


        if(!inCombat){
            lassoCombatAiming.SetActive(false);
        }



        if(lassoActive && holdingItem == false){
            startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
            if(internalCooldown <= 0){
                DrawLassoLine();
            }
        }
        LassoAiming();


        if(drawToLasso){
            if(projectile != null){
                connectPoint = projectile.transform.Find("ConnectPoint");
                
                /*lineStart = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y + 1.5f,
                gameObject.transform.position.z)
                + (gameObject.transform.forward * 0.25f);*/
                drawToLassoLine.enabled = true;


                Vector3[] lineConPos = new Vector3[3];
                lineConPos[0] = lassoHipLocation.transform.position;
                lineConPos[1] = lassoHandLocation.transform.position;
                lineConPos[2] = connectPoint.position;
                drawToLassoLine.positionCount = lineConPos.Length;
                drawToLassoLine.SetPositions(lineConPos);


                /*drawToLassoLine.positionCount = 2;
                drawToLassoLine.SetPosition(0, lassoHipLocation.transform.position);
                drawToLassoLine.SetPosition(1, connectPoint.position);*/
            }
        }
        else if(drawToLasso == false){
            drawToLassoLine.enabled = false;
        }

        if(spinning != null && spinningConnectPoint != null){
            /*drawToLassoLine.SetPosition(0, lassoHipLocation.transform.position);
            drawToLassoLine.SetPosition(1, lassoHandLocation.transform.position);
            drawToLassoLine.SetPosition(2, spinningConnectPoint.position);*/
            Vector3[] lineConPos = new Vector3[3];
            lineConPos[0] = lassoHipLocation.transform.position;
            lineConPos[1] = lassoHandLocation.transform.position;
            lineConPos[2] = spinningConnectPoint.position;
            drawToLassoLine.positionCount = lineConPos.Length;
            drawToLassoLine.SetPositions(lineConPos);
            spinningLasso.GetComponent<Collider>().enabled = false;
        }
    }

    private void CancelAiming(){
        animator.SetBool("isLassoing", false);
        drawToLasso = false;
        lineRend.enabled = false;
        drawToLassoLine.enabled = false;
        cancelAim = true;
        Destroy(spinningLasso);
    }

}
