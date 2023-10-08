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

    [Header("For Lasso Specific Aiming")]
    [SerializeField] Transform launchPoint;
    [SerializeField] LineRenderer lineRend;
    //[SerializeField] Transform releasePos;
    [SerializeField] [Range(10,100)] private int linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float tBetween = 0.1f;
    [SerializeField] GameObject lassoObject;
    [SerializeField] [Range(1, 100)] private int lassoLaunchStrength;
    void Update(){
        if(Input.GetMouseButtonDown(1)){
            /*startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
            aimAngle = aimController.GetAimAngle();
            lassoActive = true;
            CastRay();*/
            LaunchLasso();
        }
        if(Input.GetMouseButtonUp(1)){
            lassoActive = false;
        }

        if(lassoActive){
            startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
            aimAngle = aimController.GetAimAngle();
        }


    }

    /*private void CastRay(){
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
    }*/

    /*private void DrawLassoLine(){
        lineRend.enabled = true;
        lineRend.positionCount = Mathf.CeilToInt(linePoints / tBetween) + 1;

        int i = 0;

        lineRend.SetPosition(i, startPos);
        for(float time = 0; time < linePoints; time += tBetween){
            i++;
            Vector3 point  = startPos + time * aimAngle;
            point.y = startPos.y + aimAngle.y * time + (Physics.gravity.y / 2f * time * time);

            lineRend.SetPosition(i, point);
        }


        
    }*/

    private void LaunchLasso(){
        var projectile = Instantiate(lassoObject, launchPoint.position, launchPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = lassoLaunchStrength * aimAngle.up;
    }
}
