using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DeadEye : MonoBehaviour {
    public enum DeadEyeState
    {
        off,
        targeting,
        shooting
    };
    // Create an instance of DeadEyeState as a variable
    private DeadEyeState deadEyeState = DeadEyeState.off;
    // List of assigned targets
    private List<Transform> targets = new List<Transform>();
    // An array of the cross UI, the small "x" indicator for the targets
    public Transform[] cross;
    public CameraController camLook;
    // The animator component of your gun
    public Animator anim;
    // Timer for the gun to cooldown, you can link it to your current gun's script
    private float cooldownTimer = 0;
    // The audio source that contains the gun shot sound
    public AudioSource shot_sfx;

    private void Update()
    {
        // Update timer
        if (cooldownTimer > 0.0f)
            cooldownTimer -= Time.deltaTime;
        else
            cooldownTimer = 0.0f;

        // Aim (Hold Right Click) - Enter DeadEye
        if (Input.GetButtonDown("Fire2"))
        {
            // Enter targeting mode if it's off
            if (deadEyeState == DeadEyeState.off)
            {
                deadEyeState = DeadEyeState.targeting;
            }
        }

        // Fire (Left Click) - If DeadEye, SetTarget. Else just Fire()
        if (Input.GetButtonDown("Fire1"))
        {
            // If we're not in the DeadEye mode, fire a single shot
            if (deadEyeState == DeadEyeState.off)
                Fire();

            // If we're in targeting state in the DeadEye mode, then we will assign a new target
            if (deadEyeState == DeadEyeState.targeting)
            {
                // Setting targets
                // Storing the collision info into a new variable "hit"
                RaycastHit hit;
                // Check if we hit an object starting from our position, going straight forward with a max distance of 100 units
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
                {
                    // Creating a temporary GameObject to store the target info
                    GameObject tmpTarget = new GameObject();
                    // Assign the target position to it
                    tmpTarget.transform.position = hit.point;
                    // Attach it to the target (child of the target) so it updates its position if the target is moving
                    tmpTarget.transform.parent = hit.transform;
                    // Add its transform to our List of targets
                    targets.Add(tmpTarget.transform);
                }
            }
        }

        // Release (Release Right Click)
        if (Input.GetButtonUp("Fire2"))
        {
            // If we're in 'targeting' mode, we will go to 'shooting' mode
            if (deadEyeState == DeadEyeState.targeting)
                deadEyeState = DeadEyeState.shooting;
        }
    }

    private void FixedUpdate()
    {
        UpdateState();
        UpdateTargetUI();
    }

    private void UpdateState()
    {
        // Reset if DeadEye is off
        if (deadEyeState == DeadEyeState.off)
        {
            // Enable the camera script
            camLook.enabled = true;
            // Reset time back to normal
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        // When we're in shooting mode
        else if (deadEyeState == DeadEyeState.shooting)
        {
            // Reset time
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            // Disable the camera controls
            camLook.enabled = false;
            // Updating looking at targets and shooting
            UpdateDeadEye();
        }
        // We're in targeting mode
        else
        {
            // Slow-motion
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            // Enable camera script
            camLook.enabled = true;
        }
    }

    private void UpdateTargetUI()
    {
        // Loop through all "cross" target indicators
        for (int i = 0; i < cross.Length; i++)
        {
            // If we are still within the targets we have
            if (i < targets.Count)
            {
                // Activate it
                cross[i].gameObject.SetActive(true);
                // Then update its position to the screen position of the target
                cross[i].position = Camera.main.WorldToScreenPoint(targets[i].position);
            }
            else // If we exceeded the last target
                cross[i].gameObject.SetActive(false); //Deactivate it
        }
    }

    private void Fire()
    {
        anim.SetTrigger("isShooting");
        // 1.0f / bullets per second to get the cooldown timer between each shot
        cooldownTimer = 1.0f / 2.0f;
        // Play the gun shot sfx
        shot_sfx.Play();
    }

    private void UpdateDeadEye()
    {
        // If we're in shooting state and we still have targets in our list
        if (deadEyeState == DeadEyeState.shooting && targets.Count > 0)
        {
            // Get the current target in a temporary variable of type Transform which is the first element in the list
            Transform currentTarget = targets[0];
            // Get the required rotation for our camera to be looking at the target
            Quaternion rot = Quaternion.LookRotation(currentTarget.position - transform.position);
            // Updating the camera rotation to the "Looking at target" rotation gradually
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 30 * Time.deltaTime);
            // Get the difference between our current rotation and the target's
            float diff = (transform.eulerAngles - rot.eulerAngles).magnitude;
            if (diff <= 0.1f && cooldownTimer <= 0)
            {
                Fire();
                // Remove the target form the list
                targets.Remove(currentTarget);
                // Destroy the target
                Destroy(currentTarget.gameObject);
            }
        }
        else 
            deadEyeState = DeadEyeState.off; 
    }
}
