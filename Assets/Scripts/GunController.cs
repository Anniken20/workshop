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
    //public vars
    public int maxBounces;
    public float maxDistance;

    [Tooltip("Time in seconds while player is motionless firing gun.")]
    public float fireTime;

    //private vars
    private bool canShoot = true;
    private int currBounces;
    private Vector3 bulletPos;
    private Vector3 bulletDir;

    //references
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (!canShoot) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FreezePlayerRoutine());
            print("FIRE");
            FireGun();
        }
    }

    private void FireGun()
    {
        _lineRenderer.positionCount = 1;
        currBounces = 0;
        bulletPos = gameObject.transform.position;
        bulletDir = gameObject.transform.forward;

        _lineRenderer.SetPosition(0, bulletPos);

        RaycastHit hitData;

        //loop for bounces
        while(currBounces < maxBounces)
        {
            _lineRenderer.positionCount++;

            //if hits something
            if(Physics.Raycast(bulletPos, bulletDir, out hitData, maxDistance))
            {
                //add point for bullet trail
                _lineRenderer.SetPosition(currBounces + 1, hitData.point);

                //end ricochet if you hit not-reflective
                if (hitData.collider.gameObject.tag == "NotRicochet")
                {
                    break;
                }

                //ricochet
                currBounces++;
                bulletPos = hitData.point;
                bulletDir = hitData.normal;
            } 

            //else doesn't hit something
            else
            {
                _lineRenderer.SetPosition(currBounces + 1, bulletDir * maxDistance);
                break;
            }
        }
    }

    private IEnumerator FreezePlayerRoutine()
    {
        PlayerController.main.firingGun = true;
        yield return new WaitForSeconds(fireTime);
        PlayerController.main.firingGun = false;
    }

    public void DisableShooting()
    {
        canShoot = false;

        //will eventually disable/grey out gun HUD
    }
}
