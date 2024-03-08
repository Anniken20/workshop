using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveDamageTrigger : MonoBehaviour
{
    private BoxCollider box;
    private int graveDMG;
    private void Start()
    {
        box = GetComponent<BoxCollider>();
        graveDMG = GetComponentInParent<GraveSelection>().graveDamage;
        box.enabled = false;
    }
    public void EnableCollider()
    {
        box.enabled = true;
    }
    public void DisableCollider()
    {
        box.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Daughter>();
        if(obj != null)
        {
            obj.TakeDamage(graveDMG);
            Debug.Log("Damaged Daughter");
        }
        else
        {
            Debug.Log("Not Poppy: " +other.name);
        }
    }
}
