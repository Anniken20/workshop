using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.InputSystem;
using StarterAssets;

/* Core Gun Mechanic for Ghost Moon High Noon
 * 
 * Shoot gun in isometric setup with ricochet capabilities
 * Includes option to redirect most recent bullet fired in mid-air within a time window
 * 
 * Ghost ammo of X bullets. Takes time before bullets return back to player
 * 
 * 
 * Closely related to BulletController script and AimController script
 * 
 * Caden Henderson
 * 9/25/23
 * 
 */

public class GunController : MonoBehaviour
{
    public CharacterMovement iaControls;

    private InputAction shoot;
    private InputAction redirect;
    public GameObject bulletPrefab;
    public AimController aimController;
    
    [Header("Additional Animations")]
    private Animator anim;
    [Tooltip("Attach the main camera here")]
    public CinemachineVirtualCamera playerFollowCam;
    [Tooltip("Attach the PlayeraCameraRoot object, a child of the Player")]
    public GameObject playerCamRoot;

    [Tooltip("Time in seconds while player is motionless firing gun.")]
    public float fireTime;
    [Tooltip("For fireTime seconds after firing, the player will speed will multiply by this factor.")]
    public float reducedSpeedFactor = 0.5f;
    [Tooltip("The amount of time the player has to redirect the shot. " +
        "\nIf time elapses, the bullet slips away at its current redirect angle.")]
    public float lunaWindowTime;
    public int maxGhostAmmo = 5;

    //hidden vars
    [HideInInspector] public bool canShoot = true;
    private Vector3 aimAngle;
    private GameObject mostRecentBullet;
    private bool lunaMode;
    private Animator animator;
    private ThirdPersonController thirdPersonController;
    private int ghostAmmo;
    private BulletHUD bulletHUD;

    private void Start()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        if(reducedSpeedFactor <= 0)
        {
            reducedSpeedFactor = 0.5f;
            Debug.LogWarning("reducedSpeedFactor field of GunController on Player was 0. Set to 0.5 Teehee!");
        }

        ghostAmmo = maxGhostAmmo;

        bulletHUD = FindObjectOfType<BulletHUD>();
        bulletHUD.StartBulletHUD(ghostAmmo);
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (PauseMenu.paused) return;
        if (thirdPersonController._inDialogue) return;
        
        //get angle data from 1 script, so it will be consistent across lasso/gun
        aimAngle = aimController.GetAimAngle();

        //luna redirection / complete luna redirection
        if (redirect.triggered)
        {
            //if in luna redirect mode, then complete redirect.
            //else enter luna mode
            if (lunaMode)
            {
                FinishRedirect();
                return;
            }

            RedirectBullet();
        }

        if (shoot.triggered)
        {
            //dont allow more bullets fired if in luna redirection mode
            if (lunaMode)
            {
                FinishRedirect();
                return;
            }

            if (!canShoot) return;

            //otherwise fire new bullet
            FireGun();
        }
    }

    private void FireGun()
    {
        if (!HasAmmo())
        {
            Misfire();
            return;
        }

        //slow player down temporarily
        StartCoroutine(FreezePlayerRoutine());

        //animation trigger
        anim = GetComponent<Animator>();
        anim.Play("Shoot");

        //instantiate and fire bullet
        GameObject bullet = Instantiate(bulletPrefab);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.mainCamera = playerFollowCam;
        bulletController.playerCamRoot = playerCamRoot;
        bulletController.player = gameObject;
        bulletController.Fire(gameObject.transform, aimAngle);

        //store this so we know which bullet to redirect
        mostRecentBullet = bullet;

        //pass in the information of the main cam, target, and player
        //so we know how to modify properly
        
        //anim = GetComponent<Animator>();
        //anim.SetBool("isShooting", false);

        //trigger animation trigger
        //animator.SetTrigger("shootTrigger");
        ghostAmmo--;
        bulletHUD.SubtractBulletHUD();

    }

    //slow down the player speed by some factor
    private IEnumerator FreezePlayerRoutine()
    {
        thirdPersonController.ChangeSpeedByFactor(reducedSpeedFactor);
        canShoot = false;
        yield return new WaitForSeconds(fireTime);
        canShoot = true;
        thirdPersonController.ChangeSpeedByFactor(1/reducedSpeedFactor);
    }

    public void DisableShooting()
    {
        canShoot = false;
        //will eventually disable/grey out gun HUD
    }

    public void ReenableShooting()
    {
        canShoot = true;
        //will eventually make our HUD yay
    }

    private void RedirectBullet()
    {
        if (mostRecentBullet != null  && mostRecentBullet.GetComponent<BulletController>().canBeRedirected)
        {
            lunaMode = true;
            //Debug.Log("Entered bullet redirect");
            mostRecentBullet.GetComponent<BulletController>().EnterLunaMode();

            //this routine kicks the player out of redirect mode after X seconds
            StartCoroutine(LunaWindowRoutine());
        }
    }
    private void FinishRedirect()
    {
        lunaMode = false;
        //Debug.Log("Finished luna's redirect");
        if (mostRecentBullet == null) return;
        else mostRecentBullet.GetComponent<BulletController>().Redirect();

        //set to null so we can't redirect same bullet >1
        mostRecentBullet = null;
    }

    //routine for kicking the player out of redirect mode if they take too long
    private IEnumerator LunaWindowRoutine()
    {
        float remainingTime = lunaWindowTime;
        while (remainingTime > 0)
        {
            if(!lunaMode)
            {
                //kick out
                yield break;
            }


            remainingTime -= Time.deltaTime;

            //wait a frame before resuming while loop
            yield return null;
        }

        FinishRedirect();
    }

    public void RestoreBullet()
    {
        ghostAmmo++;
        bulletHUD.AddBulletHUD();
    }

    private void Misfire()
    {
        Debug.Log("You don't have enough bullets.");
    }

    private bool HasAmmo()
    {
        return ghostAmmo > 0;
    }


    private void Awake(){
        iaControls = new CharacterMovement();
    }
    private void OnEnable(){
        shoot = iaControls.CharacterControls.Shoot;
        redirect = iaControls.CharacterControls.Redirect;

        shoot.Enable();
        redirect.Enable();
    }
    private void OnDisable(){
        shoot.Disable();
        redirect.Disable();
    }
}
