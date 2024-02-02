using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    public float maxDistance;
    public float moveSpeed;
    public void Project(Vector3 dir, float speed = -312f)
    {
        direction = dir;
        if (speed != -312) moveSpeed = speed;
        StartCoroutine(ProjectRoutine());
    }

    private IEnumerator ProjectRoutine()
    {
        float distanceTraveled = 0;
        while (distanceTraveled < maxDistance)
        {
            Vector3 moveVector = moveSpeed * (direction * Time.deltaTime);
            distanceTraveled += moveVector.magnitude;
            transform.position += moveVector;

            //wait a frame before resuming loop
            yield return null;
        }

        Break();
    }

    public void Break()
    {
        Destroy(gameObject);
    }


}
