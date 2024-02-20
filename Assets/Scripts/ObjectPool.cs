using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A script to pool objects. [maxObjectCount] objects are instantiated on Start
 * and Set inactive. 
 * Then we can "Instantiate" and "Destroy" any prefab without the performance cost of 
 * Instantiate or Destroy, just by changing the visibility of an object. 
 * 
 * This script should not be edited. 
 * Other scripts should simply pull from a pool.
 * 
 * An example of this is the GunController script, which pulls from an object pool
 * attached on the Player prefab. 
 * Anytime a bullet is fired, it is pulled from the pool, set active, 
 * has its transform reset to the appropriate position, and has a custom method called "Fire", which is called.
 * The Fire method resets the bullet's data. 
 * 
 * The [maxObjectCount] is a hard cap. 
 * you cannot make more. Pulling from the pool when all of the objects in the pool are already active
 * will return "null"
 * 
 * Caden Henderson 2/19/2024
 */

public class ObjectPool : MonoBehaviour
{
    public GameObject objectType;
    public int maxObjectCount;
    private GameObject[] objects;

    private void Start()
    {
        objects = new GameObject[maxObjectCount];
        for (int i = 0; i < objects.Length; i++)
        {
            GameObject tmp = Instantiate(objectType);
            tmp.SetActive(false);
            objects[i] = tmp;
        }
    }

    public GameObject PullFromPool()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].activeInHierarchy) return objects[i];
        }

        Debug.LogWarning("Limited by object pool");
        return null;
    }
}
