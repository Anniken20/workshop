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
            CloseShopMenu();
            SwitchGameControls(true);
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
