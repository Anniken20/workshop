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

    [Tooltip("Time in seconds while player is motionless firing gun.")]
    public float fireTime;

    //private vars
    private bool canShoot = true;
    private float angle;

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        float a = Input.GetAxis("Mouse Y");
        Mathf.Clamp(angle, -75f/360f, 75f/360f);
        angle = a;

        if (!canShoot) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FreezePlayerRoutine());
            FireGun();
        }
    }

    private void FireGun()
    {
        //calculate launch angle
        Vector3 launchAngle = gameObject.transform.forward + new Vector3(0, angle, 0);

        //instantiate and fire bullet
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<BulletController>().Fire(gameObject.transform, launchAngle);
    }

    private IEnumerator FreezePlayerRoutine()
    {
        //PlayerController.main.firingGun = true;
        yield return new WaitForSeconds(fireTime);
        //PlayerController.main.firingGun = false;
    }

    public void DisableShooting()
    {
        canShoot = false;

        //will eventually disable/grey out gun HUD
    }
}
