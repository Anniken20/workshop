using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float distanceTraveled;
    [HideInInspector] public int currBounces;
    [HideInInspector] public float currDmg;
    [HideInInspector] public bool inLunaMode;
    [HideInInspector] public CinemachineBrain cinemachineBrain;

    //inspector fields --------------------------
    [Header("Stats")]
    public float maxDistance;
    public int maxBounces;
    public float speed;

    [Header("Damage")]
    public float baseDmg = 50f;
    public float bounceDmgMultiplier = 1f;
    public float maxDmg = Mathf.Infinity;

    [Header("Luna Stats")]
    public CinemachineVirtualCamera lunaCam;
    [Tooltip("The time it takes to zoom in/out from the character to the bullet")]
    public float camMovementDuration;
    public LineRenderer aimLineRenderer;

    public void Fire(Transform source, Vector3 dir)
    {
        currDmg = baseDmg;
        direction = dir;
        StartCoroutine(BulletMove(source));
    }

    private IEnumerator BulletMove(Transform source)
    {
        currBounces = 0;
        distanceTraveled = 0f;

        //offset starting position for character height and in front of character
        position = new Vector3(source.position.x, source.position.y + 1.2f, source.position.z) + (source.forward * 0.125f);

        while (currBounces < maxBounces && distanceTraveled < maxDistance)
        {
            //if in luna mode, wait until exited
            while (inLunaMode) yield return null;

            //move bullet in its fired direction
            position = Vector3.MoveTowards(
                    position,
                    position + direction,
                    speed * Time.deltaTime);

            //track how far bullet has traveled so we know when to kill it
            distanceTraveled += speed * Time.deltaTime;

            //try to bounce. if it does, then reflect direction
            TryBounce();

            //apply movement
            gameObject.transform.position = position;

            //wait until end of frame to continue while loop
            yield return null;
        }
        Destroy(gameObject);
    }

    //check for upcoming bounce and apply
    private void TryBounce()
    {
        RaycastHit hitData;
        //if hits object, ricochet
        if (Physics.Raycast(position, direction, out hitData, 0.3f))
        {
            //try to apply damage if it's a damage-able object
            TryToApplyDamage(hitData.collider.gameObject);

            //teleport to point
            position = hitData.point;

            //reflect over the normal of the collision
            direction = Vector3.Reflect(direction, hitData.normal);

            //increment bounces
            maxBounces++;

            //multiply dmg
            currDmg *= bounceDmgMultiplier;
            currDmg = Mathf.Clamp(currDmg, 0, maxDmg);
        }
    }

    private void TryToApplyDamage(GameObject obj)
    {
        DamageController damageController;
        if (obj.gameObject.TryGetComponent<DamageController>(out damageController))
        {
            damageController.ApplyDamage(currDmg, direction);
        }
    }

    public void EnterLunaMode()
    {
        //this find by object kinda sucks but i dont have singleton behavior setup.
        //and this script is only on a prefab that is instantiated at runtime, 
        //so we can't just drag in a field unless we pass it in from the GunController or Player.
        cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

        //set transition speed for camera
        cinemachineBrain.m_DefaultBlend.m_Time = camMovementDuration;
        
        //pause bullet movement
        inLunaMode = true;

        //set at higher priority than any of the scene cameras
        //so cinemachine auto-blends to this cam
        lunaCam.Priority = 15;

        //enable line renderer
        aimLineRenderer.positionCount = 2;
        StartCoroutine(DrawLunaLineRoutine());
    }

    public void Redirect()
    {
        direction = gameObject.transform.forward;
        ExitLunaMode();
    }

    public void ExitLunaMode()
    {
        //resume bullet movement
        inLunaMode = false;

        //set transition speed for camera
        cinemachineBrain.m_DefaultBlend.m_Time = camMovementDuration;

        //set at lower priority so cinemachine auto-blends to this cam
        lunaCam.Priority = -1;

        //disable line renderer
        aimLineRenderer.positionCount = 1;
    }

    private IEnumerator DrawLunaLineRoutine()
    {
        gameObject.transform.forward = direction;
        Debug.Log("forward: " + gameObject.transform.forward);
        Debug.Log("direction: " + direction);
        while (inLunaMode)
        {
            //uses mouse to change Luna's angle
            HandleRedirectMouseInput();

            aimLineRenderer.SetPosition(0, gameObject.transform.position);
            aimLineRenderer.SetPosition(1, gameObject.transform.position + gameObject.transform.forward * 1000f);

            //wait a frame before continuing loop
            yield return null;
        }
    }

    private void HandleRedirectMouseInput()
    {
        //capture input from mouse
        float xDelta = Input.GetAxis("Mouse X");
        float yDelta = Input.GetAxis("Mouse Y");

        //apply sensitivity
        xDelta *=  3f;
        yDelta *= 3f;

        //rotate horizontal and vertical
        gameObject.transform.Rotate(new Vector3(0f, xDelta, yDelta));
    }
}  
