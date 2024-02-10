using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI interactionPrompt; 

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
            ShowPrompt();
        }
    }

   
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
