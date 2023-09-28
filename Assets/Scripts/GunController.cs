using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Gun Mechanic for Ghost Moon High Noon
 * 
 * Shoot gun in isometric setup with ricochet capabilities
 * 
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

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        //get angle data from 1 script, so it will be consistent across lasso/gun
        aimAngle = aimController.GetAimAngle();

        if (!canShoot) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FreezePlayerRoutine());
            FireGun();
        }
    }

    private void FireGun()
    {
        //calculate launch angle
        /*Vector3 launchAngle = new Vector3(gameObject.transform.forward.x, 
            gameObject.transform.forward.y, 
            gameObject.transform.forward.z) + new Vector3(aimAngle.x, aimAngle.y, aimAngle.z);*/

        //instantiate and fire bullet
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<BulletController>().Fire(gameObject.transform, aimAngle);
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
}
