using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialiteMist : MonoBehaviour
{
    [SerializeField] private float mistDuration;
    [SerializeField] private int mistDamage;
    [SerializeField] private float mistDamageCD;
    private bool canDamage = true;
    private PurgeMist purge;
    private float internalDuration;
    [HideInInspector] public bool dead;
    private void Awake()
    {
        //Destroy(this.gameObject, mistDuration);
        StartCoroutine(Dissipate());
        purge = FindObjectOfType<PurgeMist>();
        //purge.mistObjs.Add(this.gameObject);
        internalDuration = mistDuration;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && canDamage)
            {
                playerHealth.TakeDamage(mistDamage);
                canDamage = false;
                StartCoroutine(DamageCD());
            }
        }
    }
    private IEnumerator DamageCD()
    {
        yield return new WaitForSeconds(mistDamageCD);
        canDamage = true;
    }
    private IEnumerator Dissipate(){
        yield return new WaitForSeconds(mistDuration);
        //FadeOut();
        
    }
    /*public void FadeOut(){
        Debug.Log("Starting Fade");
        var col = this.gameObject.GetComponent<Renderer>().material.color;
        while(col.a >= 0){
            col.a -= 0.1f;
            this.gameObject.GetComponent<Renderer>().material.color = col;
            Debug.Log("Fading: " +col.a);
        }
        purge.mistObjs.Remove(this.gameObject);
        Destroy(this.gameObject);

    }*/
    private void FixedUpdate(){
        if(!dead){
            internalDuration -= Time.deltaTime;
            var color =  this.gameObject.GetComponent<Renderer>().material.color;
            color.a = internalDuration/mistDuration;
            this.gameObject.GetComponent<Renderer>().material.color = color;
            if(internalDuration <= 0){
                Destroy(this.gameObject);
            }
        }
        else{
            internalDuration -= Time.deltaTime * 6f;
            var color =  this.gameObject.GetComponent<Renderer>().material.color;
            color.a = internalDuration/mistDuration;
            this.gameObject.GetComponent<Renderer>().material.color = color;
            if(internalDuration <= 0){
                Destroy(this.gameObject);
            }
        }
    }
    public void Death(){
        dead = true;
    }
}
