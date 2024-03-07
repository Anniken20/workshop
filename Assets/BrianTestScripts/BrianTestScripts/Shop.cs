using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Shop : Interactable
{
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private int cost = 1; // Editable in Inspector
    [SerializeField]
    private Button buyButton; // Assign in the Inspector
    [SerializeField]
    private Button closeButton; // Assign in the Inspector
    [SerializeField]
    private AudioSource audioSource; // Ensure this is set up in the inspector


    protected override void Awake()
    {
        shopMenu.SetActive(false);
    }

    private void Update()
    {
        if (interactionPrompt.gameObject.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShopMenu(true);
        }
    }

    public void ToggleShopMenu(bool show)
    {
        shopMenu.SetActive(show); // Toggle visibility

        if (show)
        {
            SwitchGameControls(false);
            CoinCollector.Instance.ShowCoinsUIInstant(); 
        }
        else
        {
            SwitchGameControls(true);
            CloseShopMenu();
            CoinCollector.Instance.HideCoinsUIInstant();
        }
    }


    public void SpendCoin()
    {
        if (CoinCollector.Instance != null && CoinCollector.coinsCollected >= cost)
        {
            CoinCollector.Instance.SpendCoin(cost);
            audioSource.Play(); 
        }
    }
    
    public void BuySecretKey()
    {
        // Check if player has enough coins and doesn't already own the secret key
        if (CoinCollector.Instance != null && CoinCollector.coinsCollected >= cost && !InventoryManager.Instance.inventoryItems.Contains(InventoryManager.AllItems.SecretKey))
        {
            SpendCoin(); 
            InventoryManager.Instance.AddItem(InventoryManager.AllItems.SecretKey); // Adds the secret key to the inventory
            Debug.Log("Secret Key Purchased!");
        }
         else
        {
            Debug.Log("Not enough coins or key already purchased.");
        }
}


    private void SwitchGameControls(bool state)
    {
        if (state)
        {
            PauseMenu.main.UnPauseNoUI();
            //PauseMenu.main.PauseNoUI();
        } 
        else
        {
            PauseMenu.main.PauseNoUI();
            //PauseMenu.main.UnPauseNoUI();
        }
    }

    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
    }


}
