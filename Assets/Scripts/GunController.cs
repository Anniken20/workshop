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

public class Bullet
{
    public Vector3 position;
    public Vector3 direction;
    public float distanceTraveled;
    public float maxDistance;
    public int currBounces;
    public int maxBounces;
    public float speed;
    public int bulletIndex;

    public Bullet(Vector3 pos, Vector3 dir)
    {
        position = pos;
        direction = dir;
    }

    public Bullet(int index, float speed, int bounces, float dist)
    {
        bulletIndex = index;
        this.speed = speed;
        currBounces = bounces;
        distanceTraveled = dist;
    }

    public Bullet() { }

    public IEnumerator BulletMove(Transform source, LineRenderer lineRenderer)
    {
        Bullet bullet = new Bullet();
        bullet.currBounces = 0;
        bullet.position = new Vector3(source.position.x, 2f, source.position.z);
        bullet.direction = new Vector3(source.forward.x, 0f, source.forward.z);
        bullet.distanceTraveled = 0f;

        while (bullet.currBounces < maxBounces && bullet.distanceTraveled < maxDistance)
        {
            //move bullet in its fired direction
            bullet.position = Vector3.MoveTowards(
                    bullet.position,
                    bullet.position + bullet.direction,
                    speed * Time.deltaTime);


            //track how far bullet has traveled so we know when to kill it
            bullet.distanceTraveled += speed * Time.deltaTime;

            RaycastHit hitData;

            //if hits object, ricochet
            if (Physics.Raycast(bullet.position, bullet.direction, out hitData, 0.01f))
            {
                bullet.direction = hitData.normal;
                maxBounces++;
            }

            //update bullet trail line
            lineRenderer.SetPosition(bulletIndex, bullet.position);

            //wait until end of frame to continue while loop
            yield return null;
        }

        Debug.Log("bullet stopped");
    }
}


public class GunController : MonoBehaviour
{
    //public vars
    public int maxBounces;
    public float maxDistance;
    public float bulletSpeed;

    [Tooltip("Time in seconds while player is motionless firing gun.")]
    public float fireTime;

    //private vars
    private bool canShoot = true;
    Bullet headBullet;
    Bullet trailingBullet;

    //references
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        headBullet = new Bullet(1, bulletSpeed, maxBounces, maxDistance);
        trailingBullet = new Bullet(0, bulletSpeed, maxBounces, maxDistance);
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
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, new Vector3(gameObject.transform.position.x, 2f, gameObject.transform.position.z));

        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        StartCoroutine(headBullet.BulletMove(gameObject.transform, _lineRenderer));

        //delay to create separation between front and back of bullet trail
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(trailingBullet.BulletMove(gameObject.transform, _lineRenderer));
    }



    /*
    private void FireGunOld()
    {
        _lineRenderer.positionCount = 1;
        currBounces = 0;
        bulletFirePos = gameObject.transform.position;
        bulletFirePos = new Vector3(bulletFirePos.x, 2f, bulletFirePos.z);
        bulletDir = gameObject.transform.forward;
        bulletDir = new Vector3(bulletDir.x, 0f, bulletDir.z);

        _lineRenderer.SetPosition(0, bulletFirePos);

        RaycastHit hitData;

        //loop for bounces
        while(currBounces < maxBounces)
        {
            _lineRenderer.positionCount++;

            //if hits something
            if(Physics.Raycast(bulletFirePos, bulletDir, out hitData, maxDistance))
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
    */

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
