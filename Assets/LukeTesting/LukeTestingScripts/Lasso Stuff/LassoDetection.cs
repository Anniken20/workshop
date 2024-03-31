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
    public AudioClip lassoGrabSound;
    private AudioSource lassoSounds;
    private bool playMissOnce;
    private bool allowPickup;
    public ParticleSystem hitParticles;
    bool triggerParticlesOnce = true;
    bool detectCollOnce = true;
    Vector3 otherExtents;
    [HideInInspector] public float lassoGravity;
    Rigidbody rb;
    //[SerializeField] float xVariaton;
    //[SerializeField] float zVariation;

    void Update()
    {
    
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
        lassoSounds = GetComponent<AudioSource>();
        //StartCoroutine(LassoLifespan());

        lassoController = FindObjectOfType<LassoController>();
        grappleScript = FindObjectOfType<LassoGrappleScript>();
        player = FindObjectOfType<LassoController>();
        pickupScript = FindObjectOfType<LassoPickupScript>();

        lassoAttachPoint = lassoController.lassoAttachPoint;
        rb = GetComponent<Rigidbody>();

    }
    private void OnTriggerEnter(Collider other){
        if(detectCollOnce = true){
            var objLayer = other.gameObject.layer;
            string layerName = LayerMask.LayerToName(objLayer);

            ILassoable lassoable = other.gameObject.GetComponent<ILassoable>();
            //IGrappleable grappleable = GetComponent<IGrappleable>();
            if(lassoable != null){
                PlayFX(transform.position);
                //hitParticles.Play();
                //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, lassoGrabSound);
                lassoSounds.PlayOneShot(lassoGrabSound);
                //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, lassoedSound);
                hitObject = true;
                otherExtents = other.bounds.extents;
                if(transform.position.y>= (otherExtents.y) * 2){
                    
                    GetComponent<Rigidbody>().isKinematic = true;
                    onObject = true;
                    otherObject = other.gameObject;
                    lassoable.Lassoed(lassoAttachPoint, lassoActive, other.gameObject);
                    lassoController.holdingItem = true;
                }
            }
            else if(other.gameObject.CompareTag("Grapple")){
                PlayFX(transform.position);
                //hitParticles.Play();
                //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, lassoGrabSound);
                lassoSounds.PlayOneShot(lassoGrabSound);
                onObject = true;
                otherObject = other.gameObject;
                //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, grappleSound);
                grappleScript.Grappled(lassoActive, other.transform.gameObject);
                hitObject = true;
                //Destroy(gameObject);
                GetComponent<Rigidbody>().isKinematic = true;
                lassoController.drawToLasso = false;
                lassoController.drawToLassoLine.enabled = false;
                lassoController.holdingItem = true;
                player.GetComponent<LassoGrappleScript>().lassoConnectPoint = lassoAttachPoint;
                player.GetComponent<LassoGrappleScript>().triggerGrapOnce = true;

            }
            else if(other.gameObject.tag != "Player" && onObject == false && other.gameObject.GetComponent<OutOfBoundsScript>() == null && layerName != "AimLayer" && layerName != "BulletPassThrough"|| other.gameObject.tag != "Player" && onObject == true && otherObject.GetComponent<LassoPickupScript>().manipulateObject && other.gameObject.GetComponent<OutOfBoundsScript>() == null && layerName != "AimLayer" && layerName != "BulletPassThrough"){
                //hitParticles.Play();
                PlayFX(transform.position);
                lassoController.drawToLasso = false;
                lassoController.drawToLassoLine.enabled = false;
                //Destroy(gameObject);
                playMissOnce = true;
                recall = true;
                if(otherObject != null){
                    otherObject.GetComponent<LassoPickupScript>().DropObject();
                }
            }
            detectCollOnce = false;
        }
    }
    private void PlayFX(Vector3 location){
        if(triggerParticlesOnce){
            //hitParticles.PlayParticleSystemAtPoint(location.position);
            var particlesLocation = hitParticles.gameObject;
            particlesLocation.transform.position = location;
            hitParticles.Play();
        triggerParticlesOnce = false;
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
                //AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, missSound);
                lassoSounds.PlayOneShot(missSound);
                playMissOnce = false;
            }
        }
        if(onObject || recall){
            this.GetComponent<Collider>().enabled = false;
        }
        if(player.spinningLasso == null){
            RotateTowardsPlayer();
        }
        lassoAttachPoint = lassoController.lassoAttachPoint;
        if(onObject){
            var xScale = otherObject.transform.localScale.x -0.25f;//+ xVariaton; //+ .15f;
            var zScale = otherObject.transform.localScale.z -0.6f;//+ zVariation; //+ .15f;
            var scaleRatio = Mathf.Abs(xScale / zScale);
            if(scaleRatio <= 4.070){
                Debug.Log("Below Cutoff Scale: " +scaleRatio);
                transform.localScale = new Vector3(xScale, transform.localScale.y, zScale);
                Vector3 heldPos = new Vector3(otherObject.transform.position.x, otherObject.transform.position.y + otherExtents.y, otherObject.transform.position.z);
                transform.position = heldPos;
            }
            else{
                Debug.Log("Else Scale: " +scaleRatio);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //this.transform.position = new Vector3(transform.position.x, transform.position.y - 1.4f, transform.position.z);
                transform.position = otherObject.transform.position;
                transform.Rotate(0, 0, 90);

            }
            //Vector3 heldPos = new Vector3(otherObject.transform.position.x, otherObject.transform.position.y + otherExtents.y, otherObject.transform.position.z);
            //transform.position = otherObject.transform.position;
            //transform.position = heldPos;

        }

        //Controls the speed that the lasso falls
        rb.AddForce(Physics.gravity * lassoGravity, ForceMode.Acceleration);
    }

    private void RotateTowardsPlayer(){
        Vector3 aDirection = transform.position - player.gameObject.transform.position;
            Quaternion aRotation = Quaternion.LookRotation(aDirection);
            transform.rotation = aRotation;
            var rotation = transform.rotation;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = rotation;
            if(otherObject != null){
                if(!pickupScript.manipulateObject && player.GetComponent<LassoGrappleScript>().grapple != true && player.GetComponent<LassoGrappleScript>().canLasso == true){
                    otherObject.transform.rotation = transform.rotation;
                }
                else if(player.GetComponent<LassoGrappleScript>().grapple != true && player.GetComponent<LassoGrappleScript>().grappling != true && player.GetComponent<LassoGrappleScript>().canLasso == true){
                    transform.rotation = otherObject.transform.rotation;
                }
            }
    }
    
}
