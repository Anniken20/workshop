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

    public override string ToString()
    {
        return "index: " + bulletIndex + ", pos: " + position + ", bounces: " + currBounces;
    }

    public IEnumerator BulletMove(Transform source, LineRenderer lineRenderer)
    {
        currBounces = 0;
        position = new Vector3(source.position.x, 2f, source.position.z);
        direction = new Vector3(source.forward.x, 0f, source.forward.z);
        distanceTraveled = 0f;

        while (currBounces < maxBounces && distanceTraveled < maxDistance)
        {
            //move bullet in its fired direction
            position = Vector3.MoveTowards(
                    position,
                    position + direction,
                    speed * Time.deltaTime);


            //track how far bullet has traveled so we know when to kill it
            distanceTraveled += speed * Time.deltaTime;

            RaycastHit hitData;

            //if hits object, ricochet
            if (Physics.Raycast(position, direction, out hitData, 0.01f))
            {
                direction = hitData.normal;
                maxBounces++;
            }

            //update bullet trail line
            lineRenderer.SetPosition(bulletIndex, position);

            //wait until end of frame to continue while loop
            yield return null;
        }
        if(currBounces < maxBounces)
        {
            Debug.Log("bullet stopped due to bounces");
        }
        if (distanceTraveled < maxDistance)
        {
            Debug.Log("bullet stopped due to bounces");
        }

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
        Debug.Log("Head: " + headBullet);
        Debug.Log("Tail: " + trailingBullet);
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
