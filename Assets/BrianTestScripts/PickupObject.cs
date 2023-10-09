using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{

    public int keyCount;

    void OnTriggerEnter(Collider collider)
    {
        if(GetComponent<Collider>().gameObject.tag == "Key")
        {
            keyCount += 1;
            Destroy(gameObject);
        }
    }
}
