using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    //public CharacterMovement iaControls;
    public PlayerInput _playerInput;
    private InputAction interact;
    [SerializeField] float interactionRange;
    private AimController aimController;
    private Transform rayLaunchPoint;
    private Vector3 rayAngle;
    private Interactable interactable;
    private void Awake(){
        //iaControls = new CharacterMovement();
        _playerInput = GameObject.Find("_iaManager").GetComponent<PlayerInput>();
    }

    private void Start()
    {
        aimController = GetComponent<AimController>();
    }
    private void FixedUpdate(){
        rayLaunchPoint = aimController.shootPoint;
        rayAngle = aimController.GetAimAngle();
    }
    void Update()
    {
        if(interact.triggered){
            //ShootInteractRay();
            TryInteraction();
        }
    }

    private void OnEnable(){
        //interact = iaControls.CharacterControls.Interact;
        interact = _playerInput.actions["Interact"];

        interact.Enable();
    }
    private void OnDisable(){
        interact.Disable();
    }
    private void ShootInteractRay(){
        /*
        RaycastHit hit;
        if(Physics.Raycast(rayLaunchPoint.position, rayAngle, out hit, interactionRange)){
            IInteractable interactable = hit.transform.gameObject.GetComponent<IInteractable>();
            if(interactable != null){
                interactable.Interacted();
            }
        }
        */


    }

    private void TryInteraction()
    {
        if(interactable != null)
        {
            IInteractable[] inters = interactable.gameObject.GetComponents<IInteractable>();
            foreach (IInteractable i in inters)
                i.Interacted();
        }
    }

    public void SendInteractable(Interactable inter)
    {
        interactable = inter;
    }
}