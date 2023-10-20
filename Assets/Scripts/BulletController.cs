using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using StarterAssets;
using UnityEngine.InputSystem;

public class BulletController : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction look;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float distanceTraveled;
    [HideInInspector] public int currBounces;
    [HideInInspector] public float currDmg;
    [HideInInspector] public bool inLunaMode;
    [HideInInspector] public CinemachineBrain cinemachineBrain;
    [HideInInspector] public CinemachineVirtualCamera mainCamera;
    [HideInInspector] public GameObject playerCamRoot;
    [HideInInspector] public GameObject player;
    private float trailRendererTime;
    private float formerCamOrthoSize;

    //inspector fields --------------------------
    [Header("Stats")]
    public float maxDistance;
    public int maxBounces;
    public float speed;

    [Header("Damage")]
    public float baseDmg = 50f;
    public float bounceDmgMultiplier = 1f;
    public float maxDmg = Mathf.Infinity;

    [Header("Luna Stats")]
    public GameObject luna;
    public CinemachineVirtualCamera lunaCam;
    [Tooltip("The time it takes to zoom in/out from the character to the bullet")]
    public float camMovementDuration;
    public LineRenderer aimLineRenderer;
    [Tooltip("Whether or not the camera completely switches to Luna's \"POV\"")]
    public bool lunaPOVCam;
    [Tooltip("Orthographic size of camera if NOT in lunaPOVCam. 1->4 are reasonable values.")]
    public float lunaCamOrthoSize;
    [Tooltip("When NOT in lunaPOVCam, the time it takes to zoom in and out.")]
    public float camZoomTime;

    public void Fire(Transform source, Vector3 dir)
    {
        currDmg = baseDmg;
        direction = dir;
        StartCoroutine(BulletMove(source));
    }

    private IEnumerator BulletMove(Transform source)
    {
        currBounces = 0;
        distanceTraveled = 0f;

        //offset starting position for character height and in front of character
        position = new Vector3(source.position.x, source.position.y + 1.2f, source.position.z) + (source.forward * 0.125f);

        while (currBounces < maxBounces && distanceTraveled < maxDistance)
        {
            //if in luna mode, wait until exited
            //while (inLunaMode) yield return null;

            //move bullet in its fired direction
            position = Vector3.MoveTowards(
                    position,
                    position + direction,
                    speed * Time.deltaTime);

            //track how far bullet has traveled so we know when to kill it
            distanceTraveled += speed * Time.deltaTime;

            //try to bounce. if it does, then reflect direction
            TryBounce();

            //apply movement
            gameObject.transform.position = position;

            //wait until end of frame to continue while loop
            yield return null;
        }
        Destroy(gameObject);
    }

    //check for upcoming bounce and apply
    private void TryBounce()
    {
        RaycastHit hitData;
        //if hits object, ricochet
        //scale with speed because higher speed means more likely to clip through walls
        //but on low speed the jump to the wall is noticeable
        if (Physics.Raycast(position, direction, out hitData, speed * 0.15f))
        {
            //try to apply damage if it's a damage-able object
            TryToApplyDamage(hitData.collider.gameObject);

            //teleport to point to prevent inconsistency from sometimes bouncing early
            position = hitData.point;

            //reflect over the normal of the collision
            direction = Vector3.Reflect(direction, hitData.normal);

            //increment bounces
            maxBounces++;

            //multiply dmg
            currDmg *= bounceDmgMultiplier;
            currDmg = Mathf.Clamp(currDmg, 0, maxDmg);
        }
    }

    private void TryToApplyDamage(GameObject obj)
    {
        DamageController damageController;
        if (obj.gameObject.TryGetComponent<DamageController>(out damageController))
        {
            damageController.ApplyDamage(currDmg, direction);
        }
    }

    public void EnterLunaMode()
    {
        //show luna
        luna.SetActive(true);

        //lock movement
        player.GetComponent<ThirdPersonController>()._lunaLocked = true;

        //lock aiming
        player.GetComponent<AimController>().inLuna = true;

        //this find by object kinda sucks but i dont have singleton behavior setup.
        //and this script is only on a prefab that is instantiated at runtime, 
        //so we can't just drag in a field unless we pass it in from the GunController or Player.
        if (lunaPOVCam) cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

        //set transition speed for camera
        if (lunaPOVCam) cinemachineBrain.m_DefaultBlend.m_Time = camMovementDuration;

        //zoom main cam to look at bullet
        if (!lunaPOVCam)
        {
            mainCamera.Follow = gameObject.transform;
            formerCamOrthoSize = mainCamera.m_Lens.OrthographicSize;

            //tween camera ortho size to zoom in
            DOTween.To(() => mainCamera.m_Lens.OrthographicSize, 
                x => mainCamera.m_Lens.OrthographicSize = x, 
                lunaCamOrthoSize, camZoomTime).SetEase(Ease.OutCubic);
            
            //insta snap
            //mainCamera.m_Lens.OrthographicSize = lunaCamOrthoSize;
        }

        //pause bullet movement
        inLunaMode = true;

        //set at higher priority than any of the scene cameras
        //so cinemachine auto-blends to this cam
        if (lunaPOVCam) lunaCam.Priority = 15;

        //save trail renderer time value
        //then make it super large so trail doesn't go away
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRendererTime = trailRenderer.time;
        trailRenderer.time *= 10f;

        //while in luna mode, make bullet move slowly
        speed = speed / 10f;

        //enable line renderer
        aimLineRenderer.positionCount = 2;
        StartCoroutine(DrawLunaLineRoutine());
    }

    public void Redirect()
    {
        direction = gameObject.transform.forward;
        ExitLunaMode();
    }

    public void ExitLunaMode()
    {
        //hide luna
        luna.SetActive(false);

        //unlock our player movement
        player.GetComponent<ThirdPersonController>()._lunaLocked = false;

        //unlock our player aiming
        player.GetComponent<AimController>().inLuna = false;

        //resume bullet movement
        inLunaMode = false;

        if (lunaPOVCam)
        {
            //set transition speed for camera
            cinemachineBrain.m_DefaultBlend.m_Time = camMovementDuration;

            //set at lower priority so cinemachine auto-blends to this cam
            lunaCam.Priority = -1;
        }
        else
        {
            //reset main cam to look at player
            mainCamera.Follow = playerCamRoot.transform;
            DOTween.To(() => mainCamera.m_Lens.OrthographicSize,
                x => mainCamera.m_Lens.OrthographicSize = x,
                formerCamOrthoSize, camZoomTime).SetEase(Ease.OutCubic);

            //insta snap
            //mainCamera.m_Lens.OrthographicSize = formerCamOrthoSize;
        }

        //reset trailrenderer to previously saved value
        GetComponent<TrailRenderer>().time = trailRendererTime;

        //reset bullet speed to normal
        speed *= 10f;

        //disable line renderer
        aimLineRenderer.positionCount = 1;
    }

    private IEnumerator DrawLunaLineRoutine()
    {
        gameObject.transform.forward = direction;
        //Debug.Log("forward: " + gameObject.transform.forward);
        //Debug.Log("direction: " + direction);
        while (inLunaMode)
        {
            //uses mouse to change Luna's angle
            HandleRedirectMouseInput();

            aimLineRenderer.SetPosition(0, gameObject.transform.position);
            aimLineRenderer.SetPosition(1, gameObject.transform.position + gameObject.transform.forward * 1000f);

            //wait a frame before continuing loop
            yield return null;
        }
    }

    private void HandleRedirectMouseInput()
    {
        //capture input from mouse
        var looking = look.ReadValue<Vector2>();
        float xDelta = looking.x;
        float yDelta = looking.y;

        //apply sensitivity
        xDelta *=  3f;
        yDelta *= 3f;

        //failed
        //gameObject.transform.forward += new Vector3(0f, yDelta, 0f);

        //rotate horizontal and vertical
        gameObject.transform.Rotate(new Vector3(0f, xDelta, 0f));

        //failed
        //gameObject.transform.Rotate(new Vector3(yDelta, 0f, 0f));
    }
    private void Awake(){
        iaControls = new CharacterMovement();
    }
    private void OnEnable(){
        look = iaControls.CharacterControls.Look;

        look.Enable();
    }
    private void OnDisable(){
        look.Disable();
    }
}  
