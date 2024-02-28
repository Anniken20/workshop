using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "ScriptableObjects/DialogueData")]
[System.Serializable]
public class DialogueData : ScriptableObject
{
    private static DialogueData _instance;
    public static DialogueData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DialogueData>("DialogueData");
                if (_instance == null)
                {
                    Debug.LogError("DialogueData asset not found. Make sure it exists in a Resources folder.");
                }
            }
            return _instance;
        }
    }

    // Ensure that the object persists across scenes
    private void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    // Dialogue commented out since should be taking from Cadens already made stuff
    //public string speakerName;
    //public string[] dialogueLines;

    // Text size and color
    public int textSize = 24;
    public Color textColor = Color.white;

    // Subtitles
    public bool subtitlesOn = true;

    // Font selection
    public bool useDyslexicFont = false;
    public TMP_FontAsset defaultFont; // Default font
    public TMP_FontAsset dyslexicFont; // Font for dyslexic mode

    // Screen Shake toggle
    //public bool screenShakeEnabled;

    // Post-processing toggles
    //public bool bloomEnabled;
    //public bool chromaticAberrationEnabled;
    //public bool vignetteEnabled;

    public TMP_FontAsset GetDialogueFont()
    {
        if (useDyslexicFont && dyslexicFont != null)
            return dyslexicFont;
        else
            return defaultFont; // Use default font if dyslexia mode is off or dyslexicFont is not set
    }
}
