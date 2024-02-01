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
    public LayerMask aimLayer;
    public bool useAimReticle = true;
    [Tooltip("The distance the reticle will be drawn from collision.")]
    public float reticleDistFromCollision;
    public bool drawReticleWhenNoCollision = true;
    [Tooltip("The reticle will draw this far from the player when no collision is detected." +
        " (Only if drawReticleWhenNoCollision is enabled)")]
    public float reticleDistFromPlayer = 5f;

    [Header("Bonus Options")]
    public bool snapAngle;
    [Range(0f, 1f)]
    public float snapIncrements;
    public bool showBounceAim;
    public float bounceAimDist;

    [Header("IK Setup")]
    private Animator animator;
    public bool activeIK;
    [HideInInspector] public Transform lookAtTarget;
    [HideInInspector] public Transform lookAtRotator;
    public Transform lookPoint;
    public Camera cam;

    [HideInInspector] public bool canAim = true;
    [HideInInspector] public bool inLuna = false;
    private Vector3 angle;
    private Vector3 angleWithIntensity;
    private bool showingAimLine = false;
    [HideInInspector] public float sensitivity = 5f;
    [HideInInspector] public bool fireOnMouseUp;

    [Header("Debug")]
    public bool drawAimcast;
    public GameObject shootTowardsBox;

    //debug
    private Vector3 pointA;
    private Vector3 pointB;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        angle = new Vector3();
        angleWithIntensity = new Vector3();
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
        aimCursor.transform.position = Input.mousePosition;
    }

    private void Update()
    {
        //Debug.Log("Aim angle: " + angle);
        MoveCursor();
        UpdateAim();
        if (fireOnMouseUp)
        {

        } else
        {
            DrawAimReticle();
            DrawLine();
        }
    }

    private void MoveCursor()
    {
        //cringe doing this every frame but whatever ?
        var looking = look.ReadValue<Vector2>();

        aimCursor.transform.position += new Vector3(
            cursorSensitivity * looking.x, 
            cursorSensitivity * looking.y);

        //clamp cursor to screen bounds
        float xClamp = Mathf.Clamp(aimCursor.transform.position.x, 0, Screen.width);
        float yClamp = Mathf.Clamp(aimCursor.transform.position.y, 0, Screen.height);
        aimCursor.transform.position = new Vector3(xClamp, yClamp, 0f);
    }

    private void UpdateAim()
    {
        //in case locked during menus or game cutscenes etc.
        //currently referenced by BulletController when redirecting
        if (PauseMenu.paused || !canAim || inLuna)
        {
            angle = lookPoint.position - shootPoint.position;
            //angle = angle.normalized;
            SnapAngle();
            return;
        }

        Ray ray = cam.ScreenPointToRay(aimCursor.transform.position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, aimLayer);
        Vector3 goodPoint = new Vector3(hit.point.x, shootPoint.position.y, hit.point.z);


        //Vector3 camPoint = cam.ViewportToWorldPoint(aimCursor.transform.position);
        //Vector3 goodPoint = new Vector3(camPoint.x, shootPoint.position.y, camPoint.z);
        lookPoint.position = goodPoint;
        if(shootTowardsBox != null) shootTowardsBox.transform.position = goodPoint;

        //multiplying by 1000 then normalizing so that the angle isn't too small and therefore reduced to 0. 
        angle = (goodPoint - shootPoint.position);
        angle = Vector3.ClampMagnitude(angle * 1000f, 10f);
        //angle = angle.normalized;
        SnapAngle();

        //adding intensity by scaling the angle with the mouse cursor's distance from center of screen
        //taking absolute value so it doesn't flip the angle
        //dividing it by screen size so it's screen resolution independent on its intensity
        Vector3 alpha = aimCursor.transform.position - new Vector3(Screen.width / 2f, Screen.height / 2f);
        alpha.x /= Screen.width;
        alpha.y /= Screen.height;
        float intensity = Mathf.Abs(Vector3.Magnitude(alpha)) * 10f;
        angleWithIntensity = angle * intensity;
        
        //debug tools
        //pointA = ray.origin;
        //pointB = goodPoint;

        //toggle aim draw
        if (aim.triggered)
        {
            showingAimLine = !showingAimLine;
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawAimcast) return;
        Gizmos.DrawLine(pointA, pointB);
        RaycastHit h;
        Physics.Raycast(pointB, Vector3.down, out h, aimLayer);
        Gizmos.DrawLine(pointB, h.point);
    }

    public void ShowCursor()
    {
        aimCursor.SetActive(true);
    }
    public void HideCursor()
    {
        aimCursor.SetActive(false);
    }

    public void HideLine()
    {
        aimLine.positionCount = 0;
    }

    public void DrawLine()
    {
     /*   
        if (!showingAimLine)
        {
            aimLine.positionCount = 0;
            return;
        }
     */
        
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
            //ShowBulletFuture(hitData.point, Vector3.Reflect(angle, hitData.normal));
            ShowBulletFuture(hitData.point, Vector3.Reflect(angle, hitData.normal));
        }
        //otherwise set position 1 far away in that direction
        else
        {
            aimLine.SetPosition(1, angle * 500f);
        }
    }

    public void DrawAimReticle()
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
    public Vector3 GetAimAngle()
    {
        return angle;
    }

    public Vector3 GetAimAngleWithIntensity()
    {
        return angleWithIntensity;
    }

    //for drawing the line to preview a bounce
    public void ShowBulletFuture(Vector3 point, Vector3 bounceAngle)
    {
        if (!showBounceAim) return;
        aimLine.positionCount = 3;
        bounceAngle = SnapAngle(bounceAngle);
        RaycastHit hitData;
        if (Physics.Raycast(point, bounceAngle, out hitData, bounceAimDist, LayerManager.main.shootableLayers))
        {
            aimLine.SetPosition(2, hitData.point);
        }
        //otherwise set position 2 floating
        else
        {
            aimLine.SetPosition(2, point + (bounceAngle.normalized * bounceAimDist));
        }
    }

    public void SnapAngle()
    {
        if (snapAngle)
        {
            float x = Mathf.Round(angle.x / snapIncrements) * snapIncrements;
            float y = Mathf.Round(angle.y / snapIncrements) * snapIncrements;
            float z = Mathf.Round(angle.z / snapIncrements) * snapIncrements;
            angle = new Vector3(x, y, z);
        }
    }

    private Vector3 SnapAngle(Vector3 givenAngle)
    {
        float x = Mathf.Round(givenAngle.x / snapIncrements) * snapIncrements;
        float y = Mathf.Round(givenAngle.y / snapIncrements) * snapIncrements;
        float z = Mathf.Round(givenAngle.z / snapIncrements) * snapIncrements;
        return new Vector3(x, y, z);
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
                    animator.SetLookAtPosition(aimReticle.transform.position);
                }
            } else
            {
                animator.SetLookAtWeight(0);
            }
        }
    }
}
