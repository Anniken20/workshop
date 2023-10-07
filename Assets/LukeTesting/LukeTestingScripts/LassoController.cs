using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoController : MonoBehaviour

{
    public AimController aimController;
    private Vector3 aimAngle;
    private Vector3 startPos;

    private bool lassoActive;


    [SerializeField] Transform lassoAttachPoint;
    [SerializeField] Vector3 lineOrigin;

    [SerializeField] float rayDistance;

    void Update(){
        if(Input.GetMouseButtonDown(1)){
            startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
            aimAngle = aimController.GetAimAngle();
            lassoActive = true;
            CastRay();
        }
    }

    private void CastRay(){
        Ray ray = new Ray(startPos, aimAngle);
        if(Physics.Raycast(ray, out RaycastHit hit, rayDistance)){
            ILassoable lassoable = hit.transform.gameObject.GetComponent<ILassoable>();
            IGrappleable grappleable = GetComponent<IGrappleable>();
            if(lassoable != null){
                lassoable.Lassoed(lassoAttachPoint, lassoActive);
            }
            else if(hit.transform.gameObject.CompareTag("Grapple")){
                grappleable.Grappled(lassoActive, hit.transform.gameObject, lassoAttachPoint);
            }
        }
    }
}
