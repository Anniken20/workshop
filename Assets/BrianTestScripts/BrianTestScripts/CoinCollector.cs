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

    private Coroutine moveHUDRoutine;

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
        uiOffScreenPosition = new Vector2(500, coinsRectTransform.anchoredPosition.y); // Adjust this value
        uiOnScreenPosition = new Vector2(50, coinsRectTransform.anchoredPosition.y);    // Adjust this value
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

        //kill the previous move routine to reset the time remaining until moving back
        if (moveHUDRoutine != null) StopCoroutine(moveHUDRoutine);
        moveHUDRoutine = StartCoroutine(HideCoinsUIAfterDelay(5f));;
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = "" + coinsCollected;
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

