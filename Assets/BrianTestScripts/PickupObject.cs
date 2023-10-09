using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{

   public int pickup;

    void OnTriggerEnter(Collider collider)
    {
        if(GetComponent<Collider>().gameObject.tag == "Key")
        {
            pickup +=1;
            Destroy(gameObject);
        }
    }
}
