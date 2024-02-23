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

    // Dialogue
    public string speakerName;
    public string[] dialogueLines;

    // Text size and color
    public int textSize = 12;
    public Color textColor = Color.white;

    // Subtitles
    public bool subtitlesOn = true;
}