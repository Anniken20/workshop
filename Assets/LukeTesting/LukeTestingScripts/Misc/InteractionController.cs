using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public CharacterMovement iaControls;
    private InputAction interact;
    [SerializeField] private float interactionRange = 5f; // Interaction range
    private Transform playerTransform;

    private void Awake()
    {
        iaControls = new CharacterMovement();
        playerTransform = transform; 
        interact = iaControls.CharacterControls.Interact;
    }

    private void OnEnable()
    {
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    private void Update()
    {
        if (interact.triggered)
        {
            ShootInteractRay();
        }
    }

    private void ShootInteractRay()
    {
        RaycastHit hit;
        // Directly use transform.forward for the interaction direction
        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interacted();
            }
        }
    }
}
