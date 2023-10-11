using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/* Gun Mechanic for Ghost Moon High Noon
 * 
 * Shoot gun in isometric setup with ricochet capabilities
 * Includes option to redirect most recent bullet fired in mid-air
 * 
 * 
 * Caden Henderson
 * 9/25/23
 * 
 */

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public AimController aimController;
    [Tooltip("Attach the main camera here")]
    public CinemachineVirtualCamera playerFollowCam;
    [Tooltip("Attach the PlayeraCameraRoot object, a child of the Player")]
    public GameObject playerCamRoot;

    [Tooltip("Time in seconds while player is motionless firing gun.")]
    public float fireTime;
    [Tooltip("The amount of time the player has to redirect the shot. " +
        "\nIf time elapses, the bullet slips away at its current redirect angle.")]
    public float lunaWindowTime;

    //private vars
    private bool canShoot = true;
    private Vector3 aimAngle;
    private GameObject mostRecentBullet;
    private bool lunaMode;

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        //get angle data from 1 script, so it will be consistent across lasso/gun
        aimAngle = aimController.GetAimAngle();

        //luna redirection / complete luna redirection
        if (Input.GetKeyDown(KeyCode.R))
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

        if (Input.GetMouseButtonDown(0))
        {
            //dont allow more bullets fired if in luna redirection mode
            if (lunaMode)
            {
                FinishRedirect();
                return;
            }

            if (!canShoot) return;

            //otherwise fire new bullet                
            StartCoroutine(FreezePlayerRoutine());
            FireGun();
        }
    }

    private void FireGun()
    {
        //instantiate and fire bullet
        GameObject bullet = Instantiate(bulletPrefab);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Fire(gameObject.transform, aimAngle);

        //store this so we know which bullet to redirect
        mostRecentBullet = bullet;

        //pass in the information of the main cam so we know how to modify its target
        bulletController.mainCamera = playerFollowCam;
        bulletController.playerCamRoot = playerCamRoot;

    }

    private IEnumerator FreezePlayerRoutine()
    {
        //PlayerController.main.firingGun = true;
        canShoot = false;
        yield return new WaitForSeconds(fireTime);
        canShoot = true;
        //PlayerController.main.firingGun = false;
    }

    public void DisableShooting()
    {
        canShoot = false;

        //will eventually disable/grey out gun HUD
    }

    private void RedirectBullet()
    {
        if (mostRecentBullet != null)
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
        mostRecentBullet.GetComponent<BulletController>().Redirect();

        //set to null so we can't redirect same bullet >1
        mostRecentBullet = null;
    }

    //routine for kicking the player out of redirect mode if they take too long
    private IEnumerator LunaWindowRoutine()
    {
        float remainingTime = lunaWindowTime;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            //wait a frame before resuming while loop
            yield return null;
        }

        FinishRedirect();
    }
}
