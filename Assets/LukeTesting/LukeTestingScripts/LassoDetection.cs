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

    void Update()
    {
        lassoAttachPoint = lassoController.lassoAttachPoint;
    }
    void Start(){

        StartCoroutine(LassoLifespan());

        lassoController = FindObjectOfType<LassoController>();
        grappleScript = FindObjectOfType<LassoGrappleScript>();

        lassoAttachPoint = lassoController.lassoAttachPoint;

    }
    private void OnTriggerEnter(Collider other){
        ILassoable lassoable = other.gameObject.GetComponent<ILassoable>();
        //IGrappleable grappleable = GetComponent<IGrappleable>();
        if(lassoable != null){
            Vector3 otherExtents = other.bounds.extents;
            Debug.Log((otherExtents.y) * 2);
            if(transform.position.y>= (otherExtents.y) * 2){
                lassoable.Lassoed(lassoAttachPoint, lassoActive);
                Destroy(gameObject);
                lassoController.drawToLasso = false;
                lassoController.drawToLassoLine.enabled = false;
                other.GetComponent<LassoPickupScript>().lassoedObject = other.gameObject;
            }
            else{
                Destroy(gameObject);
                lassoController.drawToLasso = false;
                lassoController.drawToLassoLine.enabled = false;
            }
        }
        else if(other.gameObject.CompareTag("Grapple")){
            grappleScript.Grappled(lassoActive, other.transform.gameObject);

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
