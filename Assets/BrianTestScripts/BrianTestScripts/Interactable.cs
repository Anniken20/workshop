using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI interactionPrompt; 
    protected bool isPlayerInRange = false;

    protected virtual void Awake()
    {
        if (interactionPrompt == null)
        {
            Debug.LogError("InteractionPrompt not set on " + gameObject.name);
        }
        interactionPrompt.text = ""; 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerInRange = true;
            ShowPrompt();
        }
    }

   
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HidePrompt();
        }
    }

    protected virtual void ShowPrompt()
    {
        interactionPrompt.text = "Press 'E' to interact"; // Set prompt text
        interactionPrompt.gameObject.SetActive(true); // Make sure the prompt is visible
    }

    protected virtual void HidePrompt()
    {
        interactionPrompt.text = ""; // Clear prompt text
        interactionPrompt.gameObject.SetActive(false); // Hide the prompt
    }
}
