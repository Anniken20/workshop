using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllNearbyGhosts : MonoBehaviour
{
    public float range;
    public void KillGhostsInRange()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range);
        foreach(Collider col in cols)
        {
            if(col.gameObject.GetComponent<GhostEnemy>() != null)
            {
                Destroy(col.gameObject);
            }
        }
    }
}
