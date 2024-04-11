using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems itemType;
    [SerializeField] GameObject collectedItemImage;
    public AudioClip pickupSound;

    //pickup key
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemType);
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
