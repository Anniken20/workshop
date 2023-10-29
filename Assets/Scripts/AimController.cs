using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction look;
    private InputAction aim;
    public LineRenderer aimLine;

    [Tooltip("(Roughly) 2 => 90* of freedom. 0.5 => 30* of freedom.")]
    public float yAngleFreedom;
    public float sensitivity = 5f;
    [Range(0f, 1f)]
    public float horizontalFreedom;
    public bool horizontalRotate = true;
    public Transform shootPoint;

    [Header("IK Setup")]
    public GameObject playerHead;
    private Animator animator;
    public bool activeIK;
    public Transform lookAtTarget;
    public Transform lookAtRotator;

    public float myFloat;

    [HideInInspector] public bool canAim = true;
    [HideInInspector] public bool inLuna = false;
    private Vector3 angle;
    private Vector3 modifiedAngle;
    private bool showingAimLine = false;
    private float xDelta;
    private float yDelta;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        angle = new Vector3();
        modifiedAngle = new Vector3();
        canAim = true;
        inLuna = false;
        iaControls = new CharacterMovement();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAim();
    }

    private void FixedUpdate()
    {
        DrawLine();
    }
    private void UpdateAim()
    {
        //in case locked during menus or game cutscenes etc.
        //currently referenced by BulletController when redirecting
        if (!canAim || inLuna) return;

        //get input from mouse
        var looking = look.ReadValue<Vector2>();
        xDelta = looking.x;
        yDelta = looking.y;

        //apply sensitivity
        xDelta *= sensitivity / 1;
        yDelta *= sensitivity / 100;

        //rotate horizontal
        if(horizontalRotate)
        {
            LimitHorizontalRotation();
            lookAtRotator.Rotate(new Vector3(0f, xDelta, 0f));
        }
            
        modifiedAngle.y += yDelta;

        //clamp and save y-value 
        float ySave = Mathf.Clamp(modifiedAngle.y, -yAngleFreedom / 2, yAngleFreedom / 2);

        //restore y angle
        modifiedAngle.y = ySave;

        //apply modified angle
        //angle = gameObject.transform.forward;
        //angle += modifiedAngle;
        angle = lookAtTarget.position - shootPoint.position;

        //toggle aim draw
        if (aim.triggered)
        {
            showingAimLine = !showingAimLine;
        }

        //recenter aim
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(RecenterAimRoutine());
        }
    }

    private void DrawLine()
    {
        if (!showingAimLine)
        {
            aimLine.positionCount = 0;
            return;
        }

        //add 2 positions to line renderer so a line is drawn
        aimLine.positionCount = 2;

        //start line near chest level, a little bit in front of player
        Vector3 startPos = new Vector3(
            gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward.normalized * 0.125f);

        //set position 0 slightly in front of player
        aimLine.SetPosition(0, startPos);

        //set position 1 at impact point if collision
        RaycastHit hitData;
        if (Physics.Raycast(startPos, angle, out hitData, 200f))
        {
            aimLine.SetPosition(1, hitData.point);
        }
        //otherwise set position 1 far away in that direction
        else
        {
            aimLine.SetPosition(1, angle * 200f);
        }
    }

    private IEnumerator RecenterAimRoutine()
    {
        canAim = false;
        while (Mathf.Abs(Vector3.Magnitude(angle - gameObject.transform.forward)) > 0.0001f)
        {
            //lerp towards forward direction
            angle = Vector3.Lerp(angle, gameObject.transform.forward, 0.2f);
                
            //wait a frame until next loop iteration
            yield return null;
        }
        modifiedAngle = new Vector3(0f, 0f, 0f);
        canAim = true;
    }

    private void LimitHorizontalRotation()
    {
        float pivotRotY = lookAtRotator.transform.localRotation.y;
        if(pivotRotY < -horizontalFreedom || pivotRotY > horizontalFreedom)
        {
            float clampedRotY = Mathf.Clamp(pivotRotY * 90f, -horizontalFreedom * 90f, horizontalFreedom * 90f);
            //clampedRotY = myFloat;
            Debug.Log("Limited y rot to: " + clampedRotY);
            lookAtRotator.transform.localRotation =
                Quaternion.Euler(lookAtRotator.rotation.x,
                clampedRotY,
                0f);
            xDelta = 0f;
        }
    }

    public Vector3 GetAimAngle()
    {
        return angle;
    }
    
    private void OnEnable(){
        look = iaControls.CharacterControls.Look;
        aim = iaControls.CharacterControls.Aim;

        look.Enable();
        aim.Enable();
    }
    private void OnDisable(){
        look.Disable();
        aim.Disable();
    }

    private void OnAnimatorIK()
    {
        if (animator)
        {
            if (activeIK)
            {
                if(lookAtTarget != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookAtTarget.position);
                }
            } else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}
