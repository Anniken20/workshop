using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoController : MonoBehaviour

{
    public AimController aimController;
    private Vector3 aimAngle;
    private Vector3 startPos;
    private bool lassoActive;
    [SerializeField] public Transform lassoAttachPoint;
    //[SerializeField] Vector3 lineOrigin;
    //[SerializeField] float rayDistance;

    [Header("For Lasso Specific Aiming")]
    [SerializeField] Transform launchPoint;
    [SerializeField] LineRenderer lineRend;
    //[SerializeField] Transform releasePos;
    [SerializeField] [Range(10,100)] private int linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float tBetween = 0.1f;
    [SerializeField] GameObject lassoObject;
    [SerializeField] [Range(1f, 15f)] private float lassoLaunchStrength;
    [SerializeField] Transform lassoOrigin;
    [SerializeField] float lassoCooldown = 2f;
    public bool startLassoCooldown = true;
    public bool holdingItem;
    public bool drawToLasso;
    [SerializeField] public LineRenderer drawToLassoLine;
    private GameObject projectile;
    private LayerMask lassoAimMask;
    private void Awake(){
        int lassoLayer = lassoObject.gameObject.layer;
        for(int i = 0; i<32; i++){
            if(!Physics.GetIgnoreLayerCollision(lassoLayer, i)){
                lassoAimMask |= 1 << i;
            }
        }
    }
    void Update(){
        if(drawToLasso){
            drawToLassoLine.enabled = true;
            drawToLassoLine.SetPosition(0, startPos);
            drawToLassoLine.SetPosition(1, projectile.transform.position);
        }
        if(startLassoCooldown){
            lassoCooldown -= 1f * Time.deltaTime;
        }
        if(Input.GetMouseButtonDown(1)){
            if(holdingItem == false){
                lassoActive = true;
                
            }
        }
        if(Input.GetMouseButtonUp(1)){
            lassoActive = false;
            if(holdingItem == false && lassoCooldown <= 0){
                LaunchLasso();
                lassoCooldown = 2f;
                lineRend.enabled = false;
            }
        }

        if(lassoActive && holdingItem == false){
            startPos = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y + 1.5f,
            gameObject.transform.position.z)
            + (gameObject.transform.forward * 0.25f);
            aimAngle = aimController.GetAimAngle();
            if(lassoCooldown <= 0){
                DrawLassoLine();
            }
        }
    }


    private void DrawLassoLine(){
        lineRend.enabled = true;
        lineRend.positionCount = Mathf.CeilToInt(linePoints / tBetween) + 1;

        Vector3 lassoVelocity = lassoLaunchStrength * aimAngle / lassoObject.GetComponent<Rigidbody>().mass;

        int i = 0;

        lineRend.SetPosition(i, startPos);
        for(float time = 0; time < linePoints; time += tBetween){
            i++;
            Vector3 point  = startPos + time * lassoVelocity;
            point.y = startPos.y + lassoVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRend.SetPosition(i, point);

            Vector3 lastPosition = lineRend.GetPosition(i - 1);
            if(Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, (point - lastPosition).magnitude, lassoAimMask)){
                lineRend.SetPosition(i, hit.point);
                lineRend.positionCount = i + 1;
                return;
            }
        }
    }

    private void LaunchLasso(){
        projectile = Instantiate(lassoObject, startPos, launchPoint.rotation);
        aimAngle = aimController.GetAimAngle();
        projectile.GetComponent<Rigidbody>().AddForce(aimAngle * lassoLaunchStrength, ForceMode.Impulse);
        drawToLasso = true;
    }

    private void Start(){
        lineRend.enabled = false;
    }
}
