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
            anim.SetBool("Walking", false);
            anim.SetBool("Dead", true);
            StartCoroutine(TripDelay(s));
        }
    }

    public IEnumerator TripDelay(Salesman s)
    {
        yield return new WaitForSeconds(2f);
        s.TakeDamage(damage);
        onJordanHit?.Invoke();

    }
}
