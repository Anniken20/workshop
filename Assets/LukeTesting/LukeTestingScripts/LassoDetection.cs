using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoDetection : MonoBehaviour
{
    private LassoController lassoController;
    private LassoGrappleScript grappleScript;
    private bool lassoActive = true;
    private Transform lassoAttachPoint;
    private Vector3 lassoExtents;
    private float lassoBottom;
    [SerializeField] int lassoLifetime;
    [HideInInspector] public bool onObject;
    [HideInInspector] public bool destroy;
    private GameObject otherObject;
    private bool hitObject;
    
    void Update()
    {
        lassoAttachPoint = lassoController.lassoAttachPoint;
        if(onObject){
            transform.position = otherObject.transform.position;
        }
        if(otherObject != null){
            if(hitObject == false && otherObject.GetComponent<LassoController>().holdingItem == false){
                StartCoroutine(LassoLifespan());
                hitObject = false;
            }
        }
        if(destroy){
            Destroy(gameObject);
        }
    }
    void Start(){
        
        //StartCoroutine(LassoLifespan());

        lassoController = FindObjectOfType<LassoController>();
        grappleScript = FindObjectOfType<LassoGrappleScript>();

        lassoAttachPoint = lassoController.lassoAttachPoint;

    }
    private void OnTriggerEnter(Collider other){
        ILassoable lassoable = other.gameObject.GetComponent<ILassoable>();
        //IGrappleable grappleable = GetComponent<IGrappleable>();
        if(lassoable != null){
            hitObject = true;
            Vector3 otherExtents = other.bounds.extents;
            if(transform.position.y>= (otherExtents.y) * 2){
                
                //Destroy(gameObject);
                GetComponent<Rigidbody>().isKinematic = true;
                onObject = true;
                otherObject = other.gameObject;
                //transform.position = other.transform.position;
                //lassoController.drawToLasso = false;
                //lassoController.drawToLassoLine.enabled = false;
                lassoable.Lassoed(lassoAttachPoint, lassoActive, other.gameObject);
            }
            else{
                //Destroy(gameObject);
                //lassoController.drawToLasso = false;
                //lassoController.drawToLassoLine.enabled = false;
            }
        }
        else if(other.gameObject.CompareTag("Grapple")){
            grappleScript.Grappled(lassoActive, other.transform.gameObject);
            hitObject = true;
            Destroy(gameObject);
            lassoController.drawToLasso = false;
            lassoController.drawToLassoLine.enabled = false;
        }
        else if(other.gameObject.tag != "Player"){
            lassoController.drawToLasso = false;
            lassoController.drawToLassoLine.enabled = false;
            Destroy(gameObject);
        }
    }

    private IEnumerator LassoLifespan(){
        yield return new WaitForSeconds(lassoLifetime);
        Destroy(gameObject);
    }
    
}
