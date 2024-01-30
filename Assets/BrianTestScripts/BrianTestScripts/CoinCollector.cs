using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CoinCollector : MonoBehaviour
{
    public static CoinCollector Instance;

    public TextMeshProUGUI coinsText;
    private RectTransform coinsRectTransform;
    private static int coinsCollected = 0; 
    private Vector2 uiOffScreenPosition; // Position when the UI is off-screen
    private Vector2 uiOnScreenPosition; // Position when the UI is on-screen

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        coinsRectTransform = coinsText.GetComponent<RectTransform>();
        uiOffScreenPosition = new Vector2(coinsRectTransform.anchoredPosition.x, 50); // Adjust this value
        uiOnScreenPosition = new Vector2(coinsRectTransform.anchoredPosition.x, -24);    // Adjust this value
        DOTween.Init();
    }

    private void Start()
    {
        UpdateCoinsText();
        HideCoinsUIInstant();
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
      coinsRectTransform.DOAnchorPos(uiOnScreenPosition, 0.5f); 
    }

    private IEnumerator HideCoinsUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideCoinsUI();
    }

    private void HideCoinsUI()
    {
        coinsRectTransform.DOAnchorPos(uiOffScreenPosition, 0.5f);
    }

    private void HideCoinsUIInstant()
    {
        coinsRectTransform.anchoredPosition = uiOffScreenPosition; // Instantly place UI off-screen
    }
}

