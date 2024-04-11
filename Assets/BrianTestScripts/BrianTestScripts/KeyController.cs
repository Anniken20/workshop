using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems itemType;
     [SerializeField] GameObject collectedItemImage;
     private AudioSource audioSource;
     public AudioClip pickupSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    //pickup key
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemType);
            if (audioSource != null && pickupSound != null)
                AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, pickupSound);
            Destroy(gameObject);
             ShowCollectedItemImage(); 
             Invoke("HideCollectedItemImage", 60f);
        }
    }

    void ShowCollectedItemImage()
    {
        collectedItemImage.SetActive(true);
    }

    void HideCollectedItemImage()
    {
        collectedItemImage.SetActive(false);
    }
}
