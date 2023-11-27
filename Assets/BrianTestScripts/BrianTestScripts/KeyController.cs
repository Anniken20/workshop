using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] InventoryManager.AllItems itemType;
     [SerializeField] GameObject collectedItemImage;

    //pickup key
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemType);
            Destroy(gameObject);
             ShowCollectedItemImage(); 
        }
    }

    void ShowCollectedItemImage()
    {
        collectedItemImage.SetActive(true);
    }
}
