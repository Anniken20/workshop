using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SantanaPhase2EndTrigger : MonoBehaviour
{
    public UnityEvent onSantanaHit;
    private void OnTriggerEnter(Collider other)
    {
        Salesman s = other.gameObject.GetComponent<Salesman>();
        if (s != null)
        {
            Debug.Log("santana hit cross trigger");
            s.TakeDamage((int)(s.maxHealth / 2));
            onSantanaHit?.Invoke();
        }
    }
}
