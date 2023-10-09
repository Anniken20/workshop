using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("Time in seconds while player is motionless firing gun.")]
    public float fireTime;

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

        if (!canShoot) return;

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

            //otherwise fire new bullet                
            StartCoroutine(FreezePlayerRoutine());
            FireGun();
        }
    }

    private void FireGun()
    {
        //instantiate and fire bullet
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<BulletController>().Fire(gameObject.transform, aimAngle);

        //store this so we know which bullet to redirect
        mostRecentBullet = bullet;
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
            Debug.Log("Entered bullet redirect");
            mostRecentBullet.GetComponent<BulletController>().EnterLunaMode();
        }
    }
    private void FinishRedirect()
    {
        lunaMode = false;
        Debug.Log("Finished luna's redirect");
        mostRecentBullet.GetComponent<BulletController>().Redirect();

        //set to null so we can't redirect same bullet >1
        mostRecentBullet = null;
    }
}
