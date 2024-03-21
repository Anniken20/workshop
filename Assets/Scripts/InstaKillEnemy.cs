using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InstaKillEnemy : MonoBehaviour
{
    public int damage;
    public UnityEvent onJordanHit;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        Salesman s = other.gameObject.GetComponent<Salesman>();
        if (s != null)
        {
            Debug.Log("Jordan hit death trigger");
            s.TakeDamage(damage);
            onJordanHit?.Invoke();
            anim.SetBool("Walking", false);
            anim.SetBool("Dead", true);
        }
    }
}
