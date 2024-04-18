using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[Serializable()]
public struct UIElements
{
    [SerializeField] TextMeshProUGUI textObj;
    public TextMeshProUGUI TextObj { get { return textObj; } }

    [SerializeField] TextMeshProUGUI subscript;
    public TextMeshProUGUI Subscript { get { return subscript; } }

    [SerializeField] CanvasGroup subscriptGroup;
    public CanvasGroup SubscriptGroup { get { return subscriptGroup; } }

    [SerializeField] Image page;
    public Image Page { get { return page; } }

    [SerializeField] Image lines;
    public Image Lines { get { return lines; } }

    [SerializeField] CanvasGroup noteCanvasGroup;
    public CanvasGroup NoteCanvasGroup { get { return noteCanvasGroup; } }

    [SerializeField] CanvasGroup listCanvasGroup;
    public CanvasGroup ListCanvasGroup { get { return listCanvasGroup; } }

    [SerializeField] CanvasGroup readButton;
    public CanvasGroup ReadButton { get { return readButton; } }

    [SerializeField] CanvasGroup nextButton;
    public CanvasGroup NextButton { get { return nextButton; } }

    [SerializeField] CanvasGroup previousButton;
    public CanvasGroup PreviousButton { get { return previousButton; } }

    [SerializeField] NoteData noteDataPrefab;
    public NoteData NoteDataPrefab { get { return noteDataPrefab; } } 

    [SerializeField] RectTransform listRect;
    public RectTransform ListRect { get { return listRect; } }
}

public class NotesSystem : MonoBehaviour
{
    [SerializeField] UIElements UI = new UIElements();

    [SerializeField] Color color1 = Color.gray;
    [SerializeField] Color color2 = Color.grey;

    private static Dictionary<String, Note> Notes = new Dictionary<String, Note>();
    private List<NoteData> noteDatas = new List<NoteData>();
    private static Action<Note> A_Display = delegate { };

    [SerializeField] private AudioSource[] sources = null;
    [Space]
    [SerializeField] private AudioClip openNoteSFX = null;
    [SerializeField] private AudioClip closeNoteSFX = null;
    [Space]
    [SerializeField] private AudioClip[] turnPageSFXs = null;

    private Note activeNote = null;
    private Page ActivePage
    {
        get
        {
            return activeNote.Pages[currentPage];
        }
    }
    private int currentPage = 0;
    private bool readSubscript = false;
    private Sprite defaultPageTexture = null;
    private bool usingNotesSystem;

    public GameObject HUD;

    //input
    //public CharacterMovement iaControls;
    public PlayerInput _playerInput;
    private InputAction lassoPrev;
    private InputAction shootNext;
    private InputAction openN;

    private void OnEnable()
    {
        //iaControls = new CharacterMovement();
        _playerInput = GameObject.Find("_iaManager").GetComponent<PlayerInput>();
        A_Display += DisplayNote;

        //shootNext = iaControls.CharacterControls.Shoot;
        //lassoPrev = iaControls.CharacterControls.Lasso;
        shootNext = _playerInput.actions["Shoot"];
        lassoPrev = _playerInput.actions["Lasso"];
        openN = _playerInput.actions["Notes"];

        shootNext.Enable();
        lassoPrev.Enable();
    }

    private void OnDisable()
    {
        A_Display -= DisplayNote;

        shootNext.Disable();
        lassoPrev.Disable();
    }

    private void Start()
    {
        //Close(false);
        activeNote = null;
        currentPage = 0;
        readSubscript = false;

        defaultPageTexture = UI.Page.sprite;
    }

    private void Update()
    {
        if (openN.triggered)
        {
            usingNotesSystem = !usingNotesSystem;
            switch(usingNotesSystem)
            {
                case true:
                    Open();
                    break;
                case false:
                    Close(activeNote != null);
                    break;
            }
        }

        /*
        if (usingNotesSystem)
        {
            if (shootNext.WasPressedThisFrame())
            {
                Debug.Log("yep next");
                Next();
            } else if (lassoPrev.WasPressedThisFrame())
            {
                Debug.Log("yep prev");
                Previous();
            }
        }
        */
    }

    public void Open()
    {
        SwitchGameControls(false);

        UpdateList();
        UpdateCanvasGroup(true, UI.ListCanvasGroup);
    }

    public void Close(bool playSFX)
    {
        CloseNote(playSFX);
        UpdateCanvasGroup(false, UI.ListCanvasGroup);
    }

    private void SwitchGameControls(bool state)
    {
        if (state)
        {
            PauseMenu.main.UnPauseNoUI();
            //PauseMenu.main.PauseNoUI();
        } else
        {
            PauseMenu.main.PauseNoUI();
            //PauseMenu.main.UnPauseNoUI();
        }

    }

    private void DisplayNote(Note note)
    {
        if (note == null) { return; }

        SwitchGameControls(false);

        PlaySound(openNoteSFX);

        UpdateCanvasGroup(true, UI.NoteCanvasGroup);
        activeNote = note;

        DisplayPage(0);
    }

    private void DisplayPage(int page)
    {
        UI.ReadButton.interactable = activeNote.Pages[page].Type == PageType.Texture;

        if(activeNote.Pages[page].Type != PageType.Texture)
        { readSubscript = false; } else { if(readSubscript) { UpdateSubscript(); } }


        switch (activeNote.Pages[page].Type)
        {
            case PageType.Text:
                UI.Page.sprite = defaultPageTexture;
                UI.TextObj.text = activeNote.Pages[page].Text;
                break;
            case PageType.Texture:
                UI.Page.sprite = activeNote.Pages[page].Texture;
                UI.TextObj.text = string.Empty;
                break;
        }
        UpdateUI();
    }

    public static void Display(Note note)
    {
        A_Display(note);
            
    }

    public static void Display(string key)
    {
        var note = GetNote(key);
        A_Display(note);
    }

    public void CloseNote(bool playSFX)
    {
        if (playSFX)
        {
            PlaySound(closeNoteSFX);
        }

        UpdateCanvasGroup(false, UI.NoteCanvasGroup);
        OnNoteClose();
    }

    private void UpdateUI()
    {
        UI.PreviousButton.interactable = !(currentPage == 0);
        UI.NextButton.interactable = !(currentPage == activeNote.Pages.Length - 1);

        var useSubscript = ActivePage.Type == PageType.Texture && ActivePage.UseSubscript;
        UI.ReadButton.alpha = useSubscript ? (readSubscript ? .5f : 1f) : 0f;
        UpdateCanvasGroup(readSubscript, UI.SubscriptGroup);

        UI.Lines.enabled = ActivePage.DisplayLines;
    }

    private void UpdateList()
    {
        ClearList();

        var index = 0;
        var height = 0.0f;
        foreach (var note in Notes)
        {
            var color = index % 2 == 0 ? color1 : color2;

            var newNotePrefab = Instantiate(UI.NoteDataPrefab, UI.ListRect);
            noteDatas.Add(newNotePrefab);

            newNotePrefab.UpdateInfo(note.Value, color);

            newNotePrefab.Rect.anchoredPosition = new Vector2(0, height);
            height -= newNotePrefab.Rect.sizeDelta.y;

            UI.ListRect.sizeDelta = new Vector2(UI.ListRect.sizeDelta.x, height * -1);

            index++;
        }
    }

    private void UpdateSubscript()
    {
        UI.Subscript.text = readSubscript ? ActivePage.Text : string.Empty;
    }

    public void Next()
    {
        PlaySound(turnPageSFXs);

        currentPage++;
        DisplayPage(currentPage);
    }

    public void Previous()
    {
        PlaySound(turnPageSFXs);

        currentPage--;
        DisplayPage(currentPage);
    }

    public void Read()
    {
        readSubscript = !readSubscript;

        UpdateSubscript();
        UpdateUI();
    }

    private void ClearList()
    {
        foreach (var note in noteDatas)
        {
            Destroy(note.gameObject);
        }
        noteDatas.Clear();
    }

    private void OnNoteClose()
    {
        activeNote = null;
        currentPage = 0;
        readSubscript = false;
        if (!usingNotesSystem)
        {
            SwitchGameControls(true);
        }
    }

    private void UpdateCanvasGroup(bool state, CanvasGroup canvasGroup)
    {
        switch (state)
        {
            case true:
                canvasGroup.alpha = 1.0f;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
                break;
            case false:
                canvasGroup.alpha = 0.0f;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                break;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip)
        {
            sources[0].PlayOneShot(clip);
        }
    }

    private void PlaySound(AudioClip[] clips)
    {
        if (clips != null)
        {
            var sfx = clips[UnityEngine.Random.Range(0, clips.Length)];
            sources[0].PlayOneShot(sfx);
        }
    }

    public static void AddNote(string key, Note note)
    {
        if(Notes.ContainsKey(key) == false)
        {
            Notes.Add(key, note);
        }
    }

    public static Note GetNote(string key)
    {
        if (Notes.ContainsKey(key))
        {
            return Notes[key];
        }
        return null;
    }
}
