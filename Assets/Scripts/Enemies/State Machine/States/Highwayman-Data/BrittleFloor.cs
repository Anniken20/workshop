using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrittleFloor : OnPlayerHit
{
    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeAmount;
    //[SerializeField] float shakeDuration;
    [HideInInspector]
    public enum Axis
    {
        x,
        z
    };
    public Axis axis = new Axis();
    private string shakeDir;
    private Vector3 ogPos;
    private bool shake;
    [SerializeField] float breakDelay;
    [SerializeField] float reappearDelay;
    private void Start(){
        ogPos = this.transform.position;
        shakeDir = axis.ToString();
    }
    private bool canTrigger = true;
    [HideInInspector] public bool isAvailable;

     public override void HitEffect(Collision other){
        //Maybe Add some shake animation here?
        if(canTrigger){
            StartCoroutine(DisappearDelay());
            StartCoroutine(Shake());
            canTrigger = false;
        }
        
        //^^
        //Destroy(gameObject, breakDelay);
    }
    public IEnumerator Shake(){
        float elapsedTime = 0.0f;
        while(elapsedTime < breakDelay){
            shake = true;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = ogPos;
        shake = false;
    }
    public void FixedUpdate(){
        if(shake){
            if(shakeDir == "x"){
                this.transform.position = new Vector3(ogPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, ogPos.y, ogPos.z);
            }
            if(shakeDir == "z"){
                this.transform.position = new Vector3(ogPos.x, ogPos.y, ogPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
            }
        }
    }
    public IEnumerator DisappearDelay(){
        yield return new WaitForSeconds(breakDelay);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;
        isAvailable = false;
        StartCoroutine(ReappearDelay());
    }
    public IEnumerator ReappearDelay(){
        yield return new WaitForSeconds(reappearDelay);
        canTrigger = true;
        isAvailable = true;
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;

    }
}
