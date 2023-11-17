using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoDetection : MonoBehaviour
{
    private LassoController lassoController;
    private LassoGrappleScript grappleScript;
    private LassoPickupScript pickupScript;
    private bool lassoActive = true;
    private Transform lassoAttachPoint;
    private Vector3 lassoExtents;
    private float lassoBottom;
    [SerializeField] int lassoLifetime;
    [HideInInspector] public bool onObject;
    [HideInInspector] public bool destroy;
    private GameObject otherObject;
    private LassoController player;
    private bool hitObject;
    private float xScale;
    private float yScale;
    [HideInInspector] public bool recall;
    public AudioClip missSound;
    private bool playMissOnce;
    private bool allowPickup;

    
    void Update()
    {
        lassoAttachPoint = lassoController.lassoAttachPoint;
        if(onObject){
            var xScale = otherObject.transform.localScale.x + .15f;
            var zScale = otherObject.transform.localScale.z + .15f;
            transform.localScale = new Vector3(xScale, transform.localScale.y, zScale);
            transform.position = otherObject.transform.position;
            Vector3 aDirection = transform.position - player.gameObject.transform.position;
            Quaternion aRotation = Quaternion.LookRotation(aDirection);
            transform.rotation = aRotation;
            var rotation = transform.rotation;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = rotation;
            if(!pickupScript.manipulateObject && player.GetComponent<LassoGrappleScript>().grapple != true && player.GetComponent<LassoGrappleScript>().canLasso == true){
                otherObject.transform.rotation = transform.rotation;
            }
            else if(player.GetComponent<LassoGrappleScript>().grapple != true && player.GetComponent<LassoGrappleScript>().grappling != true && player.GetComponent<LassoGrappleScript>().canLasso == true){
                transform.rotation = otherObject.transform.rotation;
            }

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
        player = FindObjectOfType<LassoController>();
        pickupScript = FindObjectOfType<LassoPickupScript>();

        lassoAttachPoint = lassoController.lassoAttachPoint;

    }
    private void OnTriggerEnter(Collider other){
        ILassoable lassoable = other.gameObject.GetComponent<ILassoable>();
        //IGrappleable grappleable = GetComponent<IGrappleable>();
        if(lassoable != null){
            //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, lassoedSound);
            hitObject = true;
            Vector3 otherExtents = other.bounds.extents;
            if(transform.position.y>= (otherExtents.y) * 2){
                
                GetComponent<Rigidbody>().isKinematic = true;
                onObject = true;
                otherObject = other.gameObject;
                lassoable.Lassoed(lassoAttachPoint, lassoActive, other.gameObject);
            }
            else{
                //Destroy(gameObject);
                //lassoController.drawToLasso = false;
                //lassoController.drawToLassoLine.enabled = false;
            }
        }
        else if(other.gameObject.CompareTag("Grapple")){
            onObject = true;
            otherObject = other.gameObject;
            //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, grappleSound);
            grappleScript.Grappled(lassoActive, other.transform.gameObject);
            hitObject = true;
            //Destroy(gameObject);
            GetComponent<Rigidbody>().isKinematic = true;
            lassoController.drawToLasso = false;
            lassoController.drawToLassoLine.enabled = false;
            player.GetComponent<LassoGrappleScript>().lassoConnectPoint = lassoAttachPoint;
            player.GetComponent<LassoGrappleScript>().triggerGrapOnce = true;

        }
        else if(other.gameObject.tag != "Player" && onObject == false || other.gameObject.tag != "Player" && onObject == true && otherObject.GetComponent<LassoPickupScript>().manipulateObject){
            lassoController.drawToLasso = false;
            lassoController.drawToLassoLine.enabled = false;
            //Destroy(gameObject);
            playMissOnce = true;
            recall = true;
            if(otherObject != null){
                otherObject.GetComponent<LassoPickupScript>().DropObject();
            }
        }
    }

    private IEnumerator LassoLifespan(){
        yield return new WaitForSeconds(lassoLifetime);
        Destroy(gameObject);
    }
    private void FixedUpdate(){
        if(recall){
            player.GetComponent<LassoController>().endThrow = true;
            onObject = false;
            player.drawToLasso = true;
            player.connectPoint = transform.Find("RecallPoint");
            var lassoT = transform.Find("Lasso");
            var lassoObj = lassoT.gameObject;
            lassoObj.SetActive(false);
            var recallT = transform.Find("RecallPoint");
            var recallObj = recallT.gameObject;
            recallObj.SetActive(true);
            GetComponent<Rigidbody>().isKinematic = true;
            var hipObj = player.transform.Find("LassoHipLocation");
            transform.position = Vector3.Lerp(transform.position, hipObj.position, 5f * Time.deltaTime);
            if(Vector3.Distance(transform.position, hipObj.position) <= 1.5f){
                Destroy(gameObject);
                player.drawToLassoLine.enabled = false;
            }
            if(playMissOnce){
                //aSource.PlayOneShot(missSound);
                //aSource.Play();
                AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, missSound);
                playMissOnce = false;
            }
        }
    }
    
}
