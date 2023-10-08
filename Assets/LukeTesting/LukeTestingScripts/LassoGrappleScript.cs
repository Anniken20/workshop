using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoGrappleScript : MonoBehaviour, IGrappleable
{

    private bool grapple;
    private GameObject grapplePoint;
    [SerializeField] float breakDistance = 1.0f;
    [SerializeField] float grappleSpeed;
    [SerializeField] LineRenderer lineRend;

    private Rigidbody rb;

    [SerializeField] Transform lassoOrigin;

    public void Grappled(bool active, GameObject hitObject, Transform startPos){
        grapplePoint = hitObject;
        grapple = active;
        //rayOrigin = startPos;

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
        if(grapplePoint != null){
            float distance = Vector3.Distance(transform.position, grapplePoint.transform.position);
            if(distance <= breakDistance){
                Debug.Log("Breaking off");
                grapple = false;
                lineRend.enabled = false;
                rb.velocity = Vector3.zero;
                GetComponent<CharacterController>().enabled = true;
            }
        }
        if(grapple){
            lineRend.SetPosition(0, lassoOrigin.transform.position);
            lineRend.SetPosition(1, grapplePoint.transform.position);
        }

        if(Input.GetMouseButtonUp(1)){
            grapple = false;
            GetComponent<CharacterController>().enabled = true;
            rb.velocity = Vector3.zero;
            lineRend.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other){
        grapple = false;
        lineRend.enabled = false;
        GetComponent<CharacterController>().enabled = true;
    }
}
