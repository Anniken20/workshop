using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Isopen = false;
    public bool IsClosed = true;

    private ThirdPersonController keys;

    void Start()
    {
        keys = FindObjectOfType<ThirdPersonController>();
    }

    void Update()
    {
        if(Isopen == true && (Input.GetKeyDown(KeyCode.H)) && keys.keyCount >= 1)
        {
            //animation open
            keys.keyCount -= 1;
            Isopen = false;
            IsClosed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Isopen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       //animation exit
    }
}
