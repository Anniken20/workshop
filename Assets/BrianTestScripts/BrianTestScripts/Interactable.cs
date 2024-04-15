using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    public TextMeshProUGUI interactionPrompt; 
    protected bool isPlayerInRange = false;
    private InteractionController interactionController;

    protected virtual void Awake()
    {
        interactionController = FindObjectOfType<InteractionController>();
        if (interactionPrompt == null)
        {
            Debug.LogError("InteractionPrompt not set on " + gameObject.name);
        }
        interactionPrompt.text = "";
        HidePrompt();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerInRange = true;
            ShowPrompt();
            interactionController.SendInteractable(this);
        }
    }

   
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HidePrompt();
            interactionController.SendInteractable(null);
        }
    }

    protected virtual void ShowPrompt()
    {
        interactionPrompt.text = "'E' to interact"; // Set prompt text
        interactionPrompt.gameObject.SetActive(true); // Make sure the prompt is visible
    }

    protected virtual void HidePrompt()
    {
        interactionPrompt.gameObject.SetActive(false); // Hide the prompt
    }

    public void Interacted()
    {
        HidePrompt();
    }
}
