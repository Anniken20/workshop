using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstaKillEnemy : MonoBehaviour
{
    private Salesman s;
    //public int damage;
    private Animator anim;

    private void Start()
    {
        s = GetComponentInParent<Salesman>();
        //damage = 500;
        anim = s.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            s.TakeDamage(500);
            anim.SetBool("Walking", false);
            anim.SetBool("Dead", true);
        }
    }
}
