using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using StarterAssets;
using UnityEngine.InputSystem;
using System;
using StarterAssets;

[System.Serializable]
public struct DialogueFrame
{
    public string name;
    public Sprite portrait;
    public string message;
    [Tooltip("The time it takes between each character")]
    public float writeWaitTime;
    public UnityEvent onWriteEvent;
    public AudioClip chirp;
}

public class DialoguePopupController : MonoBehaviour, IInteractable
{

    [Header("Settings")]
    public bool onlyOnce;
    private readonly bool clickToContinue = true;
    [Tooltip("Useful for keeping the player locked even after dialogue.")]
    public bool dontUnlock;
    public bool fixHudAftewards;
    public DialogueData dialogueData; //SCRIPTABLE DATA

    [Header("Dialogue")]
    public UnityEvent onFinishedChatting;
    public DialogueFrame[] dialogues;

    private int dialogueIndex = 0;
    private Coroutine writeRoutine;
    public CharacterMovement iaControls;
    private InputAction next;
    private bool spokenTo;
    private bool inDialogue;
    private Scale HUDScaler;
    private bool writing = false;

    //to lock the player out of being stuck inside the dialogue by retriggering it immediately
    private bool canSpeak = true;
    private readonly float lockoutTime = 1f;

    private Coroutine inputRoutine;
    public AudioClip[] keyTypes;
    public AudioClip keyFinish;
    private AudioSource audioSource;

    private readonly float lockoutAfterTime = 0.5f;

    private void Start()
    {
        HUDScaler = GameObject.FindGameObjectWithTag("HUD").GetComponentInChildren<Scale>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interacted()
    {
        if (!canSpeak) return;
        if (inDialogue)
        {
            GoNext();
        } else
        {
            if(!onlyOnce || !spokenTo)
            {
                BeginSpeaking();
                inDialogue = true;
                spokenTo = true;
                if (clickToContinue) inputRoutine = StartCoroutine(InputRoutine());
            }   
        }
    }

    public void GoNext()
    {
        if (writing)
        {
            FinishLine();
            return;
        }
        dialogueIndex++;
        if(dialogueIndex >= dialogues.Length)
        {
            StopSpeaking();
        } else
        {
            DisplayDialoguePiece(dialogueIndex);
        }
    }

    public void BeginSpeaking()
    {
        //Prevent Movement
        ThirdPersonController.Main.ForceStartConversation();
        dialogueIndex = 0;
        DialogueManager.Main.gameObject.SetActive(true);
        ThirdPersonController.Main._inDialogue = true;
        DialogueManager.Main.transform.localScale = new Vector3(0f, 0f);
        DialogueManager.Main.transform.DOScale(new Vector3(1, 1), 1f).SetEase(Ease.OutExpo);
        DialogueManager.Main.dialogueBackdrop.enabled = true;
        DisplayDialoguePiece(dialogueIndex);

        //GameObject[] objs = GameObject.FindGameObjectsWithTag("UI");
        //Array.Find(objs, element => element.name == "HUD");
        HUDScaler.ScaleTo(3f);
    }
    
    public void StopSpeaking()
    {
        if (!dontUnlock) StartCoroutine(UnlockRoutine());
        DialogueManager.Main.gameObject.SetActive(false);
        if (clickToContinue) StopCoroutine(nameof(InputRoutine));
        inDialogue = false;
        StartCoroutine(EndConvo());
        onFinishedChatting.Invoke();
        if(fixHudAftewards) HUDScaler.ScaleTo(1f);
        StopCoroutine(inputRoutine);
        //Restore Movement after delay
        //if (hidePopupAfterwards) DialogueManager.Main.GetComponent<Scale>().ScaleTo(1f);
    }

    private void DisplayDialoguePiece(int i)
    {
        if (audioSource != null)
            audioSource.PlayOneShot(dialogues[i].chirp);
        dialogues[i].onWriteEvent.Invoke();
        DialogueManager.Main.characterNameText.text = dialogues[i].name;
        DialogueManager.Main.characterPortrait.sprite = dialogues[i].portrait;
        if (writeRoutine != null) StopCoroutine(writeRoutine);
        writeRoutine = StartCoroutine(WriteRoutine(dialogues[i].message, dialogues[i].writeWaitTime));

        //accessibility settings
        DialogueManager.Main.characterNameText.color = dialogueData.textColor;
        DialogueManager.Main.characterText.color = dialogueData.textColor;

       // DialogueManager.Main.characterNameText.fontSize = dialogueData.textSize;
        DialogueManager.Main.characterText.fontSize = dialogueData.textSize;
    }

    private IEnumerator WriteRoutine(string msg, float waitTime)
    {
        DialogueManager.Main.characterText.text = "";
        int i = 0;
        writing = true;
        while(i < msg.Length)
        {
            yield return new WaitForSeconds(waitTime);
            DialogueManager.Main.characterText.text += msg[i];
            if(i % 5 == 0)
            {
                if (keyTypes.Length > 0)
                {
                    // Choose a random key sound
                    AudioClip keyType = keyTypes[UnityEngine.Random.Range(0, keyTypes.Length)];

                    // Play the chosen key sound
                    if (audioSource != null)
                        audioSource.PlayOneShot(keyType);
                }
            }
            i++;
        }
        if(keyFinish != null && audioSource != null) audioSource.PlayOneShot(keyFinish);
        writing = false;
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

    private void FinishLine()
    {
        if(keyFinish != null && audioSource != null) audioSource.PlayOneShot(keyFinish);
        if (writeRoutine != null) StopCoroutine(writeRoutine);
        DialogueManager.Main.characterText.text = dialogues[dialogueIndex].message;
        writing = false;
    }

    private IEnumerator LockoutRoutine()
    {
        canSpeak = false;
        yield return new WaitForSeconds(lockoutTime);
        canSpeak = true;
    }

    private IEnumerator UnlockRoutine()
    {
        yield return new WaitForSeconds(lockoutAfterTime);
        ThirdPersonController.Main._inDialogue = false;
    }

    private void Awake()
    {
        iaControls = new CharacterMovement();
    }

    private void OnEnable()
    {
        next = iaControls.CharacterControls.Shoot;
        next.Enable();
        PauseMenu.onPause += OnPause;
        PauseMenu.onResume += OnResume;
    }

    private void OnDisable()
    {
        next.Disable();
        PauseMenu.onPause -= OnPause;
        PauseMenu.onResume -= OnResume;
    }

    private void OnPause()
    {
        next.Disable();
    }

    private void OnResume()
    {
        next.Enable();
    }

    //Restore movement after small delay
    public IEnumerator EndConvo()
    {
        yield return new WaitForSeconds(0.25f);
        if(!dontUnlock) ThirdPersonController.Main.ForceStopConversation();
    }

    public void FreezePlayer()
    {
        ThirdPersonController.Main.ForceStartConversation();
    }
}
