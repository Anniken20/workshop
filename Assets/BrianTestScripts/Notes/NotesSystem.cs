using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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

    /*[SerializeField] NoteData noteDataPrefab;
    public NoteData NoteDataPrefab { get { return noteDataPrefab; } } */

    [SerializeField] RectTransform listRect;
    public RectTransform ListRect { get { return listRect; } }
}

public class NotesSystem : MonoBehaviour
{
    [SerializeField] UIElements UI = new UIElements();

    private static Dictionary<String, Note> Notes = new Dictionary<String, Note>();

    private static Action<Note> A_Display = delegate { };

    [SerializeField] private AudioSource[] sources = null;
    [Space]
    [SerializeField] private AudioClip openNoteSFX = null;
    [SerializeField] private AudioClip closeNoteSFX = null;
    [Space]
    [SerializeField] private AudioClip[] turnPageSFXs = null;


}
