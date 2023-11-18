using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialogueFrame
{
    public Sprite portrait;
    public string message;
    public UnityEvent onWriteEvent;
}

public class DialoguePopupController : MonoBehaviour
{
    public GameObject dialogueObject;
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
        dialogueObject.SetActive(true);
        DisplayDialoguePiece(dialogueIndex);
    }
    
    public void StopSpeaking()
    {
        dialogueObject.SetActive(false);
    }

    private void DisplayDialoguePiece(int i)
    {

    }
}
