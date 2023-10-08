using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoDetection : MonoBehaviour
{
    private LassoController lassoController;
    private LassoGrappleScript grappleScript;
    private bool lassoActive = true;
    private Transform lassoAttachPoint;

    void Update()
    {
        lassoAttachPoint = lassoController.lassoAttachPoint;
    }
    void Start(){
        lassoController = FindObjectOfType<LassoController>();
        grappleScript = FindObjectOfType<LassoGrappleScript>();

        lassoAttachPoint = lassoController.lassoAttachPoint;

    }
    private void OnTriggerEnter(Collider other){
        ILassoable lassoable = other.gameObject.GetComponent<ILassoable>();
        //IGrappleable grappleable = GetComponent<IGrappleable>();
        if(lassoable != null){
            lassoable.Lassoed(lassoAttachPoint, lassoActive);
            Destroy(gameObject);
            lassoController.drawToLasso = false;
            lassoController.drawToLassoLine.enabled = false;
        }
        else if(other.gameObject.CompareTag("Grapple")){
            grappleScript.Grappled(lassoActive, other.transform.gameObject);

            Destroy(gameObject);
            lassoController.drawToLasso = false;
            lassoController.drawToLassoLine.enabled = false;
        }
    }
    
}
