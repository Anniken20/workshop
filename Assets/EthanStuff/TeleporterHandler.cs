using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterHandler : MonoBehaviour
{
    public GameObject toPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            other.gameObject.transform.position = toPoint.transform.position;
            other.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}
