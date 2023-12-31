using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction interact;
    [SerializeField] float interactionRange;
    private Transform rayLaunchPoint;
    private Vector3 rayAngle;
    private void Awake(){
        iaControls = new CharacterMovement();
    }
    private void FixedUpdate(){
        rayLaunchPoint = GetComponent<AimController>().shootPoint;
        rayAngle = GetComponent<AimController>().GetAimAngle();
    }
    void Update()
    {
        if(interact.triggered){
            ShootInteractRay();
        }
    }

    private void OnEnable(){
        interact = iaControls.CharacterControls.Interact;

        interact.Enable();
    }
    private void OnDisable(){
        interact.Disable();
    }
    private void ShootInteractRay(){
            RaycastHit hit;
            if(Physics.Raycast(rayLaunchPoint.position, rayAngle, out hit, interactionRange)){
                IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();
                if(interactable != null){
                    interactable.Interacted();
                }
            }
    }
}
