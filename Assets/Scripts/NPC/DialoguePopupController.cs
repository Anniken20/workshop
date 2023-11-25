using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using StarterAssets;
using UnityEngine.InputSystem;

[System.Serializable]
public struct DialogueFrame
{
    public Sprite portrait;
    public string message;
    [Tooltip("The time it takes between each character")]
    public float writeWaitTime;
    public UnityEvent onWriteEvent;
}

public class DialoguePopupController : MonoBehaviour, IInteractable
{
    [Header("References")]
    public GameObject dialoguePanel;
    public Image characterPortrait;
    public TMP_Text characterNameText;
    public TMP_Text characterText;
    public ThirdPersonController player;

    [Header("Settings")]
    public bool onlyOnce;
    public bool clickToContinue;

    [Header("Dialogue")]
    public string characterName;
    public UnityEvent onFinishedChatting;
    public DialogueFrame[] dialogues;

    private int dialogueIndex = 0;
    private Coroutine writeRoutine;
    public CharacterMovement iaControls;
    private InputAction next;
    private bool spokenTo;

    public void Interacted()
    {
        if (dialoguePanel.activeSelf)
        {
            GoNext();
        } else
        {
            if(!onlyOnce || !spokenTo)
            {
                BeginSpeaking();
                spokenTo = true;
                if (clickToContinue) StartCoroutine(InputRoutine());
            }   
        }
    }

    public void GoNext()
    {
        dialogueIndex++;
        if(dialogueIndex >= dialogues.Length - 1)
        {
            StopSpeaking();
        }
        DisplayDialoguePiece(dialogueIndex);
    }

    public void BeginSpeaking()
    {
        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        player._inDialogue = true;
        dialoguePanel.transform.localScale = new Vector3(0f, 0f);
        dialoguePanel.transform.DOScale(new Vector3(1, 1), 1f).SetEase(Ease.OutExpo);
        characterNameText.text = characterName;
        DisplayDialoguePiece(dialogueIndex);
    }
    
    public void StopSpeaking()
    { 
        player._inDialogue = false;
        dialoguePanel.SetActive(false);
        if (clickToContinue) StopCoroutine(nameof(InputRoutine));
        onFinishedChatting.Invoke();
    }

    private void DisplayDialoguePiece(int i)
    {
        dialogues[i].onWriteEvent.Invoke();
        characterPortrait.sprite = dialogues[i].portrait;
        if (writeRoutine != null) StopCoroutine(writeRoutine);
        writeRoutine = StartCoroutine(WriteRoutine(dialogues[i].message, dialogues[i].writeWaitTime));
    }

    private IEnumerator WriteRoutine(string msg, float waitTime)
    {
        characterText.text = "";
        int i = 0;
        while(i < msg.Length)
        {
            yield return new WaitForSeconds(waitTime);
            characterText.text += msg[i];
            i++;
        }
    }

    private IEnumerator InputRoutine()
    {
        while (true)
        {
            if (next.triggered)
            {
                Interacted();
            }

            //wait a frame
            yield return null;
        }
    }

    private void Awake()
    {
        iaControls = new CharacterMovement();
    }

    private void OnEnable()
    {
        next = iaControls.CharacterControls.Shoot;
        next.Enable();
    }

    private void OnDisable()
    {
        next.Disable();
    }
}
