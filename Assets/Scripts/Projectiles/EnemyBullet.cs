using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance = 100f;
    private float distanceTraveled;
    private int currBounces;
    [SerializeField] private int maxBounces;
    [SerializeField] private int baseDmg;
    private float currDmg;
    [SerializeField] private float dmgMultiplier;
    private GunAudioController gunAudioController;
    public bool ignoresWalls;

    private void Start()
    {
        gunAudioController = GetComponent<GunAudioController>();
        StartCoroutine(BulletTimer());
    }

    //eventually maybe we should rework this so it's by scriptable object data instead of inspector values in an enemy prefab D:
    public void Initialize(Vector3 direction, float speed, int maxBounces, float baseDmg, float dmgMultiplier)
    {
        this.direction = direction;
        this.speed = speed;
        this.maxBounces = maxBounces;
        currDmg = baseDmg;
        this.dmgMultiplier = dmgMultiplier;
    }

    public void Initialize(Vector3 direction)
    {
        this.direction = direction;
        currDmg = baseDmg;
    }

    private void Update()
    {
        Move();
        TryBounce();

        if (currBounces > maxBounces || distanceTraveled > maxDistance)
        {
            DestroyBullet();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(
                    transform.position,
                    transform.position + (direction.normalized * speed),
                    speed * Time.deltaTime);
        distanceTraveled += speed * Time.deltaTime;
    }

    private void TryBounce()
    {
        RaycastHit hitData;
        //if hits object, ricochet
        //scale with speed because higher speed means more likely to clip through walls
        //but on low speed the jump to the wall is noticeable
        if (Physics.Raycast(transform.position, direction, out hitData, speed * 0.05f))
        {
            if (hitData.collider.gameObject.CompareTag("Player"))
            {
                hitData.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)currDmg);
                Destroy(gameObject);
            }

            //draw bullethole if bullethole layer
            if (LayerManager.main.IsGunholeLayer(hitData.collider.gameObject))
            {
                transform.position = hitData.point;

                //not a decal, but kinda works for drawing a small plane
                /*
                GameObject bhole = Instantiate(bullethole, hitData.point + (hitData.normal * 0.01f),
                    Quaternion.FromToRotation(Vector3.up, hitData.normal));
                bhole.transform.parent = hitData.collider.transform;
                if (bulletholeLifetime >= 0)
                {
                    Destroy(bhole, bulletholeLifetime);
                }
                */
            }

            //phase through it if it's a pass-through layer
            if (LayerManager.main.IsPassThroughLayer(hitData.collider.gameObject))
            {
                return;
            }

            //try to apply damage if it's a damage-able object
            TryToApplyDamage(hitData.collider.gameObject);

            //try to apply interaction if it's shootable
            TryToApplyShootable(hitData.collider.gameObject);

            //destroy bullet if object is non-ricochetable
            if (!ignoresWalls && LayerManager.main.IsNoRicochetLayer(hitData.collider.gameObject))
            {
                transform.position = hitData.point;
                gunAudioController.PlayCollision();
                DestroyBullet();
                return;
            }

            //phase through it if it's a ghost object
            /*
            if (TryGetComponent<GhostController>(out GhostController ghostCon))
            {
                if (ghostCon.inGhost)
                {
                    return;
                }
            }
            */
            if (!ignoresWalls) Bounce(hitData);
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void Bounce(RaycastHit hitData)
    {
        //teleport to point to prevent inconsistency from sometimes bouncing early
        transform.position = hitData.point;

        //reflect over the normal of the collision
        direction = Vector3.Reflect(direction, hitData.normal);

        //rotate to point in right direction
        gameObject.transform.LookAt(gameObject.transform.position + (direction * 10));

        //increment bounces
        currBounces++;

        //multiply dmg
        currDmg *= dmgMultiplier;
        currDmg = Mathf.Clamp(currDmg, 0, 100f);

        //play sound
        //gunAudioController.PlayRicochet("CUTE", currBounces - 1);
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
    }

    public IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
