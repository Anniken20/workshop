using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripTrigger : MonoBehaviour
{
    [SerializeField] GameObject tpPoint;
    private Salesman s;
    private Animator anim;
    private bool canTrigger = true;
    private void FixedUpdate(){
        //Debug.Log(Vector3.Distance(s.gameObject.transform.position, this.transform.position));
    }
    private void OnTriggerEnter(Collider other){
        //Debug.Log("Hit Something: " +other.gameObject.name);
        s = other.GetComponent<Salesman>();
        if(other.gameObject && canTrigger){
            Debug.Log("Hit Salesman");
            s.GetComponent<Salesman>().enabled = false;
            int damage = (int)Mathf.Ceil(s.currentHealth);
            s.TakeDamage(damage);
            anim = s.GetComponent<Animator>();
            anim.SetBool("Dead", true);
            anim.SetBool("Running", false);
            canTrigger = false;
            StartCoroutine(Wait());
        }
    }
    private IEnumerator Wait(){
        yield return new WaitForSeconds(3);
        anim.SetBool("Dead", false);
        anim.SetBool("Idle", true);
    }
}
