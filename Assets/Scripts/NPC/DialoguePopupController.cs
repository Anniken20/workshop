using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public struct DialogueFrame
{
    public Sprite portrait;
    public string message;
    [Tooltip("The time it takes between each character")]
    public float writeWaitTime;
    public UnityEvent onWriteEvent;
}

public class DialoguePopupController : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Sprite characterPortrait;
    public TMP_Text characterNameText;
    public TMP_Text characterText;

    public string characterName;
    public DialogueFrame[] dialogues;

    private int dialogueIndex = 0;

    public void GoNext()
    {
        dialogueIndex++;
        DisplayDialoguePiece(dialogueIndex);
    }

    public void BeginSpeaking()
    {
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        characterNameText.text = characterName;
        DisplayDialoguePiece(dialogueIndex);
    }
    
    public void StopSpeaking()
    {
        dialoguePanel.SetActive(false);
    }

    private void DisplayDialoguePiece(int i)
    {
        dialogues[i].onWriteEvent.Invoke();
        characterPortrait = dialogues[i].portrait;
        StartCoroutine(WriteRoutine(dialogues[i].message, dialogues[i].writeWaitTime));
    }

    private IEnumerator WriteRoutine(string msg, float waitTime)
    {
        int i = 0;
        while(i < msg.Length)
        {
            yield return new WaitForSeconds(waitTime);
            characterText.text += msg[i];
            i++;
        }
    }
}
