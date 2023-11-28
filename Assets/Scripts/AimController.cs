using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class AimController : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction look;
    private InputAction aim;
    private InputAction recenter;
    public LineRenderer aimLine;

    [Header("General")]
    public GameObject aimCursor;
    public float cursorSensitivity = 10f;
    public Transform shootPoint;
    public GameObject aimReticle;
    public bool useAimReticle = true;
    [Tooltip("The distance the reticle will be drawn from collision.")]
    public float reticleDistFromCollision;
    public bool drawReticleWhenNoCollision = true;
    [Tooltip("The reticle will draw this far from the player when no collision is detected." +
        " (Only if drawReticleWhenNoCollision is enabled)")]
    public float reticleDistFromPlayer = 5f;

    [Header("IK Setup")]
    private Animator animator;
    public bool activeIK;
    public Transform lookAtTarget;
    public Transform lookAtRotator;
    public Transform lookAtTarget2;
    public Camera cam;

    [HideInInspector] public bool canAim = true;
    [HideInInspector] public bool inLuna = false;
    private Vector3 angle;
    private Vector3 modifiedAngle;
    private bool showingAimLine = false;
    private float xDelta;
    private float yDelta;

    private float yAngleFreedom;
    [HideInInspector] public float sensitivity = 5f;
    private float horizontalFreedom = 0.5f;
    private bool horizontalRotate = true;

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
        if (!useAimReticle) aimReticle.SetActive(false);
        else
        {
            if (reticleDistFromCollision < 0.05)
            {
                Debug.LogWarning("ReticleDistFromCollision corrected to 0.05. (Was below) Teehee!");
                reticleDistFromCollision = 0.05f;
            }
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MoveCursor();
        UpdateAim();
        DrawAimReticle();
    }

    private void FixedUpdate()
    {
        DrawLine();
    }

    private void MoveCursor()
    {
        //cringe doing this every frame but whatever ?
        var looking = look.ReadValue<Vector2>();

        aimCursor.transform.position += new Vector3(
            cursorSensitivity * looking.x, 
            cursorSensitivity * looking.y);
    }

    private void UpdateAim()
    {
        //in case locked during menus or game cutscenes etc.
        //currently referenced by BulletController when redirecting
        if (PauseMenu.paused || !canAim || inLuna)
        {
            angle = lookAtTarget.position - shootPoint.position;
            return;
        }

        //get input from mouse
        var looking = look.ReadValue<Vector2>();
        xDelta = looking.x;
        yDelta = looking.y;

        //apply sensitivity
        xDelta *= sensitivity / 2;
        yDelta *= sensitivity / 100;

        //rotate horizontal
        if(horizontalRotate)
        {
            //lookAtRotator.Rotate(new Vector3(0f, xDelta, 0f));
            //LimitHorizontalRotation();
            //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            
        }

        Ray ray = cam.ScreenPointToRay(aimCursor.transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        lookAtTarget2.position = hit.point;
        angle = hit.point - shootPoint.position;

        modifiedAngle.y += yDelta;

        //clamp and save y-value 
        float ySave = Mathf.Clamp(modifiedAngle.y, -yAngleFreedom / 2, yAngleFreedom / 2);

        //restore y angle
        modifiedAngle.y = ySave;

        //angle = lookAtTarget.position - shootPoint.position;

        //toggle aim draw
        if (aim.triggered)
        {
            showingAimLine = !showingAimLine;
        }

        //recenter aim
        if (recenter.triggered)
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

        if (PauseMenu.paused) return;

        //add 2 positions to line renderer so a line is drawn
        aimLine.positionCount = 2;

        //set position 0 slightly in front of player
        aimLine.SetPosition(0, shootPoint.position);

        //set position 1 at impact point if collision
        RaycastHit hitData;
        if (Physics.Raycast(shootPoint.position, angle, out hitData, 200f, LayerManager.main.shootableLayers))
        {
            aimLine.SetPosition(1, hitData.point);
        }
        //otherwise set position 1 far away in that direction
        else
        {
            aimLine.SetPosition(1, angle * 500f);
        }
    }

    private void DrawAimReticle()
    {
        if (!useAimReticle) return;
        if (PauseMenu.paused) return;

        bool hitSomething = Physics.Raycast(shootPoint.position, angle, out RaycastHit hitData, 100f, LayerManager.main.shootableLayers);
        if (hitSomething)
        {
            aimReticle.transform.position = hitData.point - (angle.normalized * reticleDistFromCollision);
            aimReticle.transform.rotation = Quaternion.LookRotation(hitData.normal);
        } else
        {
            if (drawReticleWhenNoCollision)
            {
                aimReticle.transform.position = shootPoint.position + angle.normalized * 3f;
                aimReticle.transform.localRotation = Quaternion.Euler(gameObject.transform.forward);
            } else
            {
                //put it really far away
                aimReticle.transform.position = shootPoint.position + angle.normalized * 1000f;
            }
        }
    }

    private IEnumerator RecenterAimRoutine()
    {
        canAim = false;
        Tween tween = lookAtRotator.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
        yield return tween.WaitForCompletion();
        canAim = true;
    }

    private void LimitHorizontalRotation()
    {
        float pivotRotY = lookAtRotator.transform.localRotation.eulerAngles.y;
        if(pivotRotY > 180f && pivotRotY < 360f &&  pivotRotY < 360f-(horizontalFreedom * 90f))
        {
            float clampedRotY = 360 - (horizontalFreedom * 90f);
            lookAtRotator.transform.localRotation =
            Quaternion.Euler(lookAtRotator.rotation.x,
                clampedRotY,
                0f);
        }
        else if(pivotRotY <= 180f && pivotRotY > 0 && pivotRotY > horizontalFreedom * 90f)
        {
            float clampedRotY = horizontalFreedom * 90f;
            lookAtRotator.transform.localRotation =
            Quaternion.Euler(lookAtRotator.rotation.x,
                clampedRotY,
                0f);
        }
    }

    public Vector3 GetAimAngle()
    {
        return angle;
    }
    
    private void OnEnable(){
        look = iaControls.CharacterControls.Look;
        aim = iaControls.CharacterControls.Aim;
        recenter = iaControls.CharacterControls.RecenterAim;

        look.Enable();
        aim.Enable();
        recenter.Enable();
    }
    private void OnDisable(){
        look.Disable();
        aim.Disable();
        recenter.Disable();
    }

    //control whether our head can turn or not
    private void OnAnimatorIK()
    {
        if (animator)
        {
            if (activeIK)
            {
                if(lookAtTarget != null)
                {
                    animator.SetLookAtWeight(1);
                    //animator.SetLookAtPosition(lookAtTarget.position);
                    animator.SetLookAtPosition(lookAtTarget2.position);
                }
            } else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}
