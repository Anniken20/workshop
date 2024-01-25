using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public static CoinCollector Instance;

    public TextMeshProUGUI coinsText;
    private static int coinsCollected = 0; 
    private static bool isUIVisible = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinsText();
        HideCoinsUI();
    }

    public void CollectCoin()
    {
        coinsCollected++;
        UpdateCoinsText();
        ShowCoinsUI();
        StartCoroutine(HideCoinsUIAfterDelay(5f));;
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coinsCollected;
        }
    }

    private void ShowCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.gameObject.SetActive(true);
            isUIVisible = true;
        }
    }

    private IEnumerator HideCoinsUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideCoinsUI();
    }

    private void HideCoinsUI()
    {
        if (coinsText != null && isUIVisible)
        {
            coinsText.gameObject.SetActive(false);
            isUIVisible = false;
        }
    }
}

