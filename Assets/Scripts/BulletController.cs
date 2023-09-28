using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float distanceTraveled;
     public float maxDistance;
    [HideInInspector] public int currBounces;
    public int maxBounces;
    public float speed;

    public void Fire(Transform source, Vector3 dir)
    {
        direction = dir;
        StartCoroutine(BulletMove(source));
    }

    private IEnumerator BulletMove(Transform source)
    {
        currBounces = 0;
        distanceTraveled = 0f;

        //offset starting position for character height and in front of character
        position = new Vector3(source.position.x, 1.2f, source.position.z) + (source.forward * 0.125f);

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
            if (Physics.Raycast(position, direction, out hitData, 0.1f))
            {
                direction = Vector3.Reflect(direction, hitData.normal);
                maxBounces++;
            }

            gameObject.transform.position = position;

            //wait until end of frame to continue while loop
            yield return null;
        }
        Destroy(gameObject);
    }
}  
