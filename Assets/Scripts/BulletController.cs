using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float distanceTraveled;
    [HideInInspector] public int currBounces;
    [HideInInspector] public float currDmg;

    //inspector fields --------------------------
    [Header("Stats")]
    public float maxDistance;
    public int maxBounces;
    public float speed;

    [Header("Damage")]
    public float baseDmg = 50f;
    public float bounceDmgMultiplier = 1f;
    public float maxDmg = Mathf.Infinity;

    public void Fire(Transform source, Vector3 dir)
    {
        currDmg = baseDmg;
        direction = dir;
        StartCoroutine(BulletMove(source));
    }

    private IEnumerator BulletMove(Transform source)
    {
        currBounces = 0;
        distanceTraveled = 0f;

        //offset starting position for character height and in front of character
        position = new Vector3(source.position.x, source.position.y + 1.2f, source.position.z) + (source.forward * 0.125f);

        while (currBounces < maxBounces && distanceTraveled < maxDistance)
        {
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
        Destroy(gameObject);
    }

    //deal damage on collision
    /*
    private void OnTriggerEnter(Collider other)
    {
        DamageController damageController;
        if(other.gameObject.TryGetComponent<DamageController>(out damageController)){
            damageController.ApplyDamage(currDmg, -direction);
        }
    }
    */

    //check for upcoming bounce and apply
    private void TryBounce()
    {
        RaycastHit hitData;
        //if hits object, ricochet
        if (Physics.Raycast(position, direction, out hitData, 0.3f))
        {
            //try to apply damage if it's a damage-able object
            TryToApplyDamage(hitData.collider.gameObject);

            //teleport to point
            position = hitData.point;

            //reflect over the normal of the collision
            direction = Vector3.Reflect(direction, hitData.normal);

            //increment bounces
            maxBounces++;

            //multiply dmg
            currDmg *= bounceDmgMultiplier;
            currDmg = Mathf.Clamp(currDmg, 0, maxDmg);
        }
    }

    private void TryToApplyDamage(GameObject obj)
    {
        DamageController damageController;
        if (obj.gameObject.TryGetComponent<DamageController>(out damageController))
        {
            damageController.ApplyDamage(currDmg, direction);
        }
    }
}  
