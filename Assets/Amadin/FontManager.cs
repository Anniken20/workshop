using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FontManager : MonoBehaviour
{
    private static FontManager _instance;

    [SerializeField] private DialogueData dialogueData;
    // Don't ask this bullshit took too fucking long
    public static FontManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("FontManager instance is null.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleDyslexiaMode(bool isEnabled)
    {
        if (dialogueData != null)
        {
            dialogueData.useDyslexicFont = isEnabled;
            UpdateAllTextUI();
        }
        else
        {
            Debug.LogWarning("DialogueData reference is null. Ensure it's assigned properly.");
        }
    }

    private void UpdateAllTextUI()
    {
        TMP_Text[] allTextComponents = Resources.FindObjectsOfTypeAll<TMP_Text>();
        foreach (TMP_Text textComponent in allTextComponents)
        {
            UpdateTextComponent(textComponent);
        }
    }

    private void UpdateTextComponent(TMP_Text textComponent)
    {
        TMP_FontAsset fontAsset = GetAppropriateFontAsset();
        if (fontAsset != null)
        {
            textComponent.font = fontAsset;
        }
    }

    private TMP_FontAsset GetAppropriateFontAsset()
    {
        if (dialogueData != null)
        {
            if (dialogueData.useDyslexicFont)
            {
                return dialogueData.dyslexicFont;
            }
            else
            {
                return dialogueData.defaultFont;
            }
        }
        else
        {
            Debug.LogWarning("DialogueData reference is null. Unable to get appropriate font asset.");
            return null;
        }
    }
}
