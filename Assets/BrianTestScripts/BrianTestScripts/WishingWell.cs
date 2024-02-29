using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WishingWell : Interactable
{
    [SerializeField]
    private int cost = 1; // Editable in Inspector
    [SerializeField]
    private AudioSource audioSource; // Ensure this is set up in the inspector

    protected override void Awake()
    {
        base.Awake();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not set on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (interactionPrompt.gameObject.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            TrySpendCoin();
        }
    }

    private void TrySpendCoin()
    {
        if (CoinCollector.Instance != null && CoinCollector.coinsCollected >= cost)
        {
            CoinCollector.Instance.SpendCoin(cost);
            audioSource.Play(); // Play coin sound effect
        }
    }
}