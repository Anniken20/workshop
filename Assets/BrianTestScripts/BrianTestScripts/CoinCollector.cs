using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CoinCollector : MonoBehaviour, IDataPersistence
{
    public static CoinCollector Instance;

    public TextMeshProUGUI coinsText;
    private RectTransform coinsRectTransform;
    public static int coinsCollected = 0; 
    private Vector2 uiOffScreenPosition; // Position when the UI is off-screen
    private Vector2 uiOnScreenPosition; // Position when the UI is on-screen

    private Coroutine moveHUDRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DOTween.Init();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        coinsText = GameObject.FindGameObjectWithTag("CoinsText")?.GetComponent<TextMeshProUGUI>();
        coinsRectTransform = coinsText?.GetComponent<RectTransform>();

        if (coinsRectTransform != null)
        {
            uiOffScreenPosition = new Vector2(660, coinsRectTransform.anchoredPosition.y);
            uiOnScreenPosition = new Vector2(451, coinsRectTransform.anchoredPosition.y);
  
            UpdateCoinsText();
            HideCoinsUIInstant(); 
        }
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

    public void ShowCoinsUIInstant()
    {
    coinsRectTransform.anchoredPosition = uiOnScreenPosition; // Instantly place UI on-screen
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

    public void HideCoinsUIInstant()
    {
        coinsRectTransform.anchoredPosition = uiOffScreenPosition; // Instantly place UI off-screen
    }

    public void LoadData(GameData data){
        coinsCollected = data.coins;
    }
    public void SaveData(ref GameData data){
        data.coins = coinsCollected;
    }

    public void SpendCoin(int amount)
    {   
        if (coinsCollected >= amount)
        {
        coinsCollected -= amount;
        UpdateCoinsText();
        ShowCoinsUI();
        
        if (moveHUDRoutine != null) StopCoroutine(moveHUDRoutine);
        moveHUDRoutine = StartCoroutine(HideCoinsUIAfterDelay(5f));
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoseCoinsOnDeath(int amount)
    {   
        coinsCollected -= amount;
        if (coinsCollected < 0) // Prevents coins from going into negative
        {
            coinsCollected = 0;
        }
        UpdateCoinsText(); 
        ShowCoinsUI(); 

        if (moveHUDRoutine != null) StopCoroutine(moveHUDRoutine);
        moveHUDRoutine = StartCoroutine(HideCoinsUIAfterDelay(5f));
        }
}

