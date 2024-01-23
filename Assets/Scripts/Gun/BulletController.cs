using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Threading;
using TMPro;

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
    [HideInInspector] public bool canBeRedirected;
    private float trailRendererTime;
    private float formerCamOrthoSize;
    private GunAudioController gunAudioController;
    private bool camFollowingBullet;

    //inspector fields --------------------------
    [Header("Stats")]
    public float maxDistance;
    public int maxBounces;
    public float speed;
    public GameObject bullethole;
    [Tooltip("After this many seconds, the bullethole will disappear. If negative, then never disappear.")]
    public float bulletholeLifetime;
    public GameObject ghostBullet;

    [Header("Damage")]
    public float baseDmg = 50f;
    public float bounceDmgMultiplier = 1f;
    public float maxDmg = Mathf.Infinity;

    [Header("Snapping Options")]
    public bool snapToBounceAngles;
    [Range(0f, 1f)]
    public float snapBounceIncrements;

    [Header("Retrieval Options")]
    [Tooltip("When the bullet collides with the player, it is instantly retrieved")]
    public bool earlyRetrieve;

    [Header("Bounce Damage Text Options")]
    public bool showDmgOnBounce;
    public TMP_Text dmgText;
    public float textPopHeight;
    public float textPopDuration;
    [Header("Higher damage => right-most color")]
    public Gradient textColGradient;
    [Header("At the lowest value, displays the left-most color")]
    public Vector2 textDmgToGradient;
    private Tween textMoveTween;
    private Tween textColorTween;

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
    [Tooltip("After this duration, the bullet can no longer be redirect.")]
    public float redirectWindowTime;
    [Tooltip("The camera will follow the redirected bullet for this many seconds before returning to look at the player.")]
    public float followBulletDuration;
    [Tooltip("The main camera will look at this transform instead of the player's transform while redirecting")]
    public Transform redirectLookPoint;
    [Tooltip("The game sleeps for this many milliseconds on redirection.")]
    public int sleepMS;
    public float shakeIntensity;
    public float shakeDuration;

    [Header("Bullet VFX")]
    public ParticleSystem sparksSystem;
    public ParticleSystem ricochetSparks;


    public void Fire(Transform source, Vector3 dir)
    {
        gunAudioController = GetComponent<GunAudioController>();
        currDmg = baseDmg;
        direction = dir;
        gameObject.transform.LookAt(gameObject.transform.position + (dir * 10));
        StartCoroutine(BulletMove(source));
        StartCoroutine(RedirectWindowRoutine());
        gunAudioController.PlayFire();
        sparksSystem.Play();
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
        gunAudioController.PlayCollision();
        DestroyBullet();
    }

    //check for upcoming bounce and apply
    private void TryBounce()
    {
        RaycastHit hitData;
        //if hits object, ricochet
        //scale with speed because higher speed means more likely to clip through walls
        //but on low speed the jump to the wall is noticeable
        if (Physics.Raycast(position, direction, out hitData, speed * 0.1f))
        {
            //phase through it if it's a pass-through layer
            if (LayerManager.main.IsPassThroughLayer(hitData.collider.gameObject))
            {
                return;
            }

            //phase through it if it's a ghost object
            if (TryGetComponent<GhostController>(out GhostController ghostCon))
            {
                if (ghostCon.inGhost)
                {
                    return;
                }
            }

            //ignore player if early retrieve is off
            if(!earlyRetrieve && hitData.collider.gameObject.CompareTag("Player"))
            {
                return;
            }

            Enemy enemy = hitData.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Enemy hit: " + enemy.name);
                enemy.TakeDamage((int)currDmg);
            }

            //draw bullethole if bullethole layer
            if (LayerManager.main.IsGunholeLayer(hitData.collider.gameObject))
            {
                transform.position = hitData.point;

                //not a decal, but kinda works for drawing a small plane
                GameObject bhole = Instantiate(bullethole, hitData.point + (hitData.normal * 0.01f),
                    Quaternion.FromToRotation(Vector3.up, hitData.normal));
                bhole.transform.parent = hitData.collider.transform;
                if (bulletholeLifetime >= 0)
                {
                    Destroy(bhole, bulletholeLifetime);
                }
            }

            IShootable shootable = hitData.collider.gameObject.GetComponent<IShootable>();
            if(shootable != null) shootable.OnShot(this);

            //try to apply damage if it's a damage-able object
            TryToApplyDamage(hitData.collider.gameObject);

            //try to apply interaction if it's shootable
            TryToApplyShootable(hitData.collider.gameObject);

            //destroy bullet if object is non-ricochetable
            if (LayerManager.main.IsNoRicochetLayer(hitData.collider.gameObject)) {
                transform.position = hitData.point;
                gunAudioController.PlayCollision();
                DestroyBullet();
                return;
            }

            Bounce(hitData);
        }
    }

    private void Bounce(RaycastHit hitData)
    {
        //teleport to point to prevent inconsistency from sometimes bouncing early
        position = hitData.point;

        //reflect over the normal of the collision
        direction = Vector3.Reflect(direction, hitData.normal);
        SnapBounceAngle();

        //rotate to point in right direction
        gameObject.transform.LookAt(gameObject.transform.position + (direction * 10));

        //increment bounces
        currBounces++;

        //multiply dmg
        currDmg *= bounceDmgMultiplier;
        currDmg = Mathf.Clamp(currDmg, 0, maxDmg);

        //play sound and vfx
        gunAudioController.PlayRicochet("CUTE", currBounces - 1);
        ricochetSparks.Play();
        //detach particle system so it doesn't follow bullet --- disabled because this breaks on second bounce ---
        //ricochetSparks.transform.parent = null;

        //show dmg numbers
        ShowBounceDmgText();
    }

    private void TryToApplyDamage(GameObject obj)
    {
        DamageController damageController;
        if (obj.TryGetComponent<DamageController>(out damageController))
        {
            damageController.ApplyDamage(currDmg, direction);
        }
    }

    private void TryToApplyShootable(GameObject obj)
    {
        ShootableController shootableController;
        if (obj.TryGetComponent<ShootableController>(out shootableController))
        {
            shootableController.OnShot();
        }
        
        IShootable[] shootables = obj.GetComponents<IShootable>();
        foreach (IShootable s in shootables){
            s.OnShot(this);
        }
    }

    public void EnterLunaMode()
    {
        //show luna
        luna.SetActive(true);
        Animator lunaAnimator = GetComponentInChildren<Animator>();
        if(lunaAnimator != null)
        {
            lunaAnimator.SetTrigger("Appear");
            Transform lunaTransform = lunaAnimator.transform;
            lunaTransform.localPosition += new Vector3(0, 0, -10);
            lunaTransform.DOLocalMove(new Vector3(0, lunaTransform.localPosition.y, 0), 0.3f);
        }

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
            mainCamera.Follow = redirectLookPoint;
            mainCamera.LookAt = redirectLookPoint;
            formerCamOrthoSize = mainCamera.m_Lens.OrthographicSize;

            //tween camera ortho size to zoom in
            DOTween.To(() => mainCamera.m_Lens.OrthographicSize, 
                x => mainCamera.m_Lens.OrthographicSize = x, 
                lunaCamOrthoSize, camZoomTime).SetEase(Ease.OutCubic);
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
        speed /= 10f;

        //enable line renderer
        aimLineRenderer.positionCount = 2;
        StartCoroutine(DrawLunaLineRoutine());

        camFollowingBullet = true;
    }

    public void Redirect()
    {
        direction = gameObject.transform.forward;
        ExitLunaMode();
    }

    public void ExitLunaMode()
    {
        //sleeeep
        Thread.Sleep(sleepMS);

        //screenshake
        ScreenShakeScript ss = mainCamera.gameObject.GetComponent<ScreenShakeScript>();
        if (ss != null) ss.ShakeCam(shakeIntensity, shakeDuration);

        //hide luna
        LunaModelExits();

        //unlock our player movement
        player.GetComponent<ThirdPersonController>()._lunaLocked = false;

        //unlock our player aiming
        player.GetComponent<AimController>().inLuna = false;

        //resume bullet movement
        inLunaMode = false;

        StartCoroutine(ResetCam());

        //reset trailrenderer to previously saved value
        GetComponent<TrailRenderer>().time = trailRendererTime;

        //reset bullet speed to normal
        speed *= 10f;

        //disable line renderer
        aimLineRenderer.positionCount = 1;
    }

    private void ExitLunaModeEarly()
    {
        //hide luna
        LunaModelExits();

        //unlock our player movement
        player.GetComponent<ThirdPersonController>()._lunaLocked = false;

        //unlock our player aiming
        player.GetComponent<AimController>().inLuna = false;

        //resume bullet movement
        inLunaMode = false;

        StartCoroutine(ResetCam(true));

        //reset trailrenderer to previously saved value
        GetComponent<TrailRenderer>().time = trailRendererTime;

        //disable line renderer
        aimLineRenderer.positionCount = 1;
    }

    private void LunaModelExits()
    {
        Animator lunaAnimator = GetComponentInChildren<Animator>();
        if (lunaAnimator != null)
        {
            lunaAnimator.SetTrigger("Bat");
            Transform lunaTransform = lunaAnimator.transform;
            lunaTransform.DOLocalMove(new Vector3(0, lunaTransform.localPosition.y, -10), 1f);
            //lunaTransform.parent = null;
            lunaTransform.DOScale(0f, 1f);
            //lunaTransform.DOLocalRotate(new Vector3(0, 0, 90), 1f, RotateMode.LocalAxisAdd);
            Destroy(lunaAnimator.gameObject, 1f);
        }
    }

    //change from luna cam to normal cam after some period of time
    private IEnumerator ResetCam(bool instant = false)
    {
        if(!instant) yield return new WaitForSeconds(followBulletDuration);
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
            mainCamera.Follow = player.transform;
            mainCamera.LookAt = player.transform;
            DOTween.To(() => mainCamera.m_Lens.OrthographicSize,
                x => mainCamera.m_Lens.OrthographicSize = x,
                formerCamOrthoSize, camZoomTime).SetEase(Ease.OutCubic);
        }
        camFollowingBullet = false;
    }

    private IEnumerator DrawLunaLineRoutine()
    {
        gameObject.transform.forward = direction;
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

    private IEnumerator RedirectWindowRoutine()
    {
        canBeRedirected = true;
        yield return new WaitForSeconds(redirectWindowTime);
        canBeRedirected = false;
    }

    private void HandleRedirectMouseInput()
    {
        if (PauseMenu.paused) return;

        //capture input from mouse
        var looking = look.ReadValue<Vector2>();
        float xDelta = looking.x;
        float yDelta = looking.y;

        //apply sensitivity
        xDelta *=  3f;
        yDelta *= 3f;

        //rotate horizontal
        gameObject.transform.Rotate(new Vector3(0f, xDelta, 0f));
    }

    private void DestroyBullet()
    {
        //instantly reset cam instead of waiting, since this script will be destroyed this frame.
        if(inLunaMode || camFollowingBullet) ExitLunaModeEarly();

        //always do this
        //if(camFollowingBullet) mainCamera.Follow = playerCamRoot.transform;

        //spawn ghost bullet
        GameObject ghost = Instantiate(ghostBullet);
        ghost.transform.position = gameObject.transform.position;
        ghost.GetComponent<GhostBulletController>().Spawn(player);

        //destroy numbers pop up - using buffer time so it doesn't disappear instantly
        dmgText.gameObject.GetComponent<DestroyAfterTime>().DestroyAfter(1f);

        //destroy this object
        Destroy(gameObject);
    }

    public bool HasBouncesRemaining()
    {
        return currBounces < maxBounces;
    }

    public void SnapBounceAngle()
    {
        if (snapToBounceAngles)
        {
            float x = Mathf.Round(direction.x / snapBounceIncrements) * snapBounceIncrements;
            float y = Mathf.Round(direction.y / snapBounceIncrements) * snapBounceIncrements;
            float z = Mathf.Round(direction.z / snapBounceIncrements) * snapBounceIncrements;
            direction = new Vector3(x, y, z);
        }
    }

    public void ShowBounceDmgText()
    {
        //kill previous tweens
        if(textMoveTween != null) textMoveTween.Kill();
        if (textColorTween != null) textColorTween.Kill();

        //set position and rotation at bullet's bounce point
        dmgText.transform.SetParent(null);
        dmgText.transform.rotation = Quaternion.identity;
        dmgText.transform.position = transform.position;

        //update text and tween text to pop up + fade out
        dmgText.text = "" + currDmg;
        textMoveTween = dmgText.transform.DOMove(dmgText.transform.position + new Vector3(0f, textPopDuration), 
            textPopDuration).SetEase(Ease.OutCubic);

        //set color based off gradient
        // --- turning dmg number to a value 0 -> based off of the textDmgToGradient bounds ---
        dmgText.color = textColGradient.Evaluate(
            Mathf.Clamp((currDmg - textDmgToGradient.x) / textDmgToGradient.y, 0f, 1f));
        textColorTween = dmgText.DOColor(new Color(dmgText.color.r, dmgText.color.g, dmgText.color.b, 0f), textPopDuration).SetEase(Ease.InCubic);

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
