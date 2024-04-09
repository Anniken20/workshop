using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FontManager : MonoBehaviour
{
    private static FontManager _instance;

    [SerializeField] private DialogueData dialogueData;

    public static FontManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FontManager>();
                if (_instance == null)
                {
                    Debug.LogError("FontManager instance is null.");
                }
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

    public void RevertToDefaultFont()
    {
        if (dialogueData != null)
        {
            dialogueData.useDyslexicFont = false;
            UpdateAllTextUI();
        }
        else
        {
            Debug.LogWarning("DialogueData reference is null. Ensure it's assigned properly.");
        }
    }

    public void UpdateAllTextUI()
    {
        // Update TextMeshPro components
        TMP_Text[] allTMPTextComponents = FindObjectsOfType<TMP_Text>(true);
        foreach (TMP_Text textComponent in allTMPTextComponents)
        {
            UpdateTextComponent(textComponent);
        }

        // Update standard Unity UI Text components
        Text[] allTextComponents = FindObjectsOfType<Text>(true);
        foreach (Text textComponent in allTextComponents)
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
            //Debug.Log(textComponent.gameObject);
        }
    }

    private void UpdateTextComponent(Text textComponent)
    {
        Debug.LogWarning("Attempting to assign TMPro font asset to UnityEngine.UI.Text. Please ensure correct font assignments.");
    }

    private TMP_FontAsset GetAppropriateFontAsset()
    {
        if (dialogueData != null)
        {
            if (dialogueData.useDyslexicFont && dialogueData.dyslexicFont != null)
            {
                return dialogueData.dyslexicFont;
            }
            else
            {
                if (!dialogueData.useDyslexicFont && dialogueData.additionalDefaultFont != null)
                {
                    return dialogueData.additionalDefaultFont;
                }
                else
                {
                    return dialogueData.defaultFont;
                }
            }
        }
        else
        {
            Debug.LogWarning("DialogueData reference is null. Unable to get appropriate font asset.");
            return null;
        }
    }
}
