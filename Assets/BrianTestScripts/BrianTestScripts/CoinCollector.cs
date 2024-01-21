using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    private static int coinsCollected = 0; // Use static variable for all instances

    private void Start()
    {
        UpdateCoinsText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        coinsCollected++;
        UpdateCoinsText();
        Destroy(gameObject);
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coinsCollected;
        }
    }

}