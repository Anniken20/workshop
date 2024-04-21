using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class Shop : Interactable, IDataPersistence
{
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private int cost = 1; // Editable in Inspector
    [SerializeField]
    private int secretKeyCost = 5;
    [SerializeField]
    private int ammoCost = 2;
    [SerializeField]
    private Button buyButton; // Assign in the Inspector
    [SerializeField]
    private Button closeButton; // Assign in the Inspector
    [SerializeField]
    private AudioSource audioSource; // Ensure this is set up in the inspector
    [SerializeField]
    private GameObject secretKeyMenu; // Assign in the Inspector
    [SerializeField]
    private GameObject ammoMenu; // Assign in the Inspector
    [SerializeField]
    private GameObject hudGameObject;


    protected override void Start()
    {
        interactionPrompt = InteractPopup.textMesh;

        if (interactionPrompt == null)
        {
            Debug.LogError("InteractionPrompt not set on " + gameObject.name);
        }
        interactionPrompt.text = "";
        shopMenu.SetActive(false);
        secretKeyMenu.SetActive(false);
        ammoMenu.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
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

    public void ToggleSecretKeyMenu(bool show)
    {
        secretKeyMenu.SetActive(show); // Toggle visibility

        if (show)
        {
            SwitchGameControls(false);
            CoinCollector.Instance.ShowCoinsUIInstant(); 
        }
        else
        {
            SwitchGameControls(true);
            CoinCollector.Instance.HideCoinsUIInstant();
        }
    }

    public void ToggleAmmoMenu(bool show)
    {
        ammoMenu.SetActive(show); // Toggle visibility

        if (show)
        {
            SwitchGameControls(false);
            CoinCollector.Instance.ShowCoinsUIInstant(); 
        }
        else
        {
            SwitchGameControls(true);
            CloseAmmoMenu();
            CoinCollector.Instance.HideCoinsUIInstant();
        }
    }


    public void SpendCoin(int cost)
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
        if (CoinCollector.Instance != null && CoinCollector.coinsCollected >= secretKeyCost && !InventoryManager.Instance.inventoryItems.Contains(InventoryManager.AllItems.SecretKey))
        {
            SpendCoin(secretKeyCost); 
            InventoryManager.Instance.AddItem(InventoryManager.AllItems.SecretKey); // Adds the secret key to the inventory
            Debug.Log("Secret Key Purchased!");

             ToggleSecretKeyMenu(false);
                ToggleAmmoMenu(false);
        }
         else
        {
            Debug.Log("Not enough coins or key already purchased.");
        }
    }

    public void BuyAmmo()
    {
        if (CoinCollector.Instance != null && CoinCollector.coinsCollected >= ammoCost)
        {
             GunController gunController = FindObjectOfType<GunController>(); 
             if (gunController != null)
             {
                SpendCoin(ammoCost);
                gunController.GhostAmmo = gunController.GhostAmmo + 1;
                Debug.Log("Ammo Purchased!");
             }
        }
        else
        {
            Debug.Log("Not enough coins.");
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

    public void CloseAmmoMenu()
    {
        ammoMenu.SetActive(false);
    }


    public void LoadData(GameData data){
        GunController gunController = FindObjectOfType<GunController>(); // Find the GunController in the scene
        if (gunController != null)
        {
            if (data.ammoCount < 6) data.ammoCount = 6;
            Debug.Log("ammoCount: " + data.ammoCount);
            gunController.GhostAmmo = data.ammoCount; // Load the saved ammo count into the GunController
        }
        else
        {
             Debug.LogError("GunController not found in the scene.");
        }
    }

    public void SaveData(ref GameData data){
        GunController gunController = FindObjectOfType<GunController>(); // Find the GunController in the scene
        if (gunController != null)
        {
            data.ammoCount = gunController.GhostAmmo; // Save the GunController's ammo count into the GameData
        }
        else
        {
             Debug.LogError("GunController not found in the scene.");
        }
    }
}
