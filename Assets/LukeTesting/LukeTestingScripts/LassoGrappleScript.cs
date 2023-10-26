using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoGrappleScript : MonoBehaviour, IGrappleable
{

    private bool grapple;
    private GameObject grapplePoint;
    [SerializeField] float breakDistance = 3.0f;
    [SerializeField] float grappleSpeed;
    [SerializeField] LineRenderer lineRend;
    private float checkCollisionDelay = .1f;
    private float internalCheckDelay;

    private Rigidbody rb;

    private Vector3 lassoOrigin;

    public void Grappled(bool active, GameObject hitObject){
        grapplePoint = hitObject;
        grapple = active;

    }

    void Start(){
        rb = GetComponent<Rigidbody>();
        lineRend.enabled = false;
    }

    void FixedUpdate(){
        if(grapple){
            StopAllCoroutines();
            rb.AddForce((grapplePoint.transform.position - transform.position) * grappleSpeed, ForceMode.VelocityChange);
            GetComponent<CharacterController>().enabled = true;
            lineRend.enabled = true;
            StartCoroutine(HookLifetime());
        }
    }

    private IEnumerator HookLifetime(){
        GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(8f);
        lineRend.enabled = false;
        grapple = false;
        rb.velocity = Vector3.zero;
        GetComponent<CharacterController>().enabled = true;
    }

    private void Update(){
        lassoOrigin = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
        if(grapplePoint != null){
            float distance = Vector3.Distance(transform.position, grapplePoint.transform.position);
            //Debug.Log(distance);
            if(distance <= breakDistance){
                //Debug.Log("Breaking off");
                grapple = false;
                lineRend.enabled = false;
                rb.velocity = Vector3.zero;
                GetComponent<CharacterController>().enabled = true;
            }
        }
        if(grapple){
            lineRend.enabled = true;
            lineRend.SetPosition(0, lassoOrigin);
            lineRend.SetPosition(1, grapplePoint.transform.position);
            internalCheckDelay -= Time.deltaTime;
        }
        else{
            internalCheckDelay = checkCollisionDelay;
        }

        if(Input.GetMouseButtonUp(1)){
            grapple = false;
            GetComponent<CharacterController>().enabled = true;
            rb.velocity = Vector3.zero;
            lineRend.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other){
        if(internalCheckDelay <= 0){
            grapple = false;
            lineRend.enabled = false;
            GetComponent<CharacterController>().enabled = true;
        }
    }
}
