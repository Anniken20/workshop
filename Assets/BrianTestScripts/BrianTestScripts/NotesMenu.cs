using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesMenu : MonoBehaviour
{
    public GameObject noteEntryPrefab; // Assign in Inspector
    public Transform notesListParent; // Assign in Inspector
    public Text noteDescriptionText; // Assign in Inspector
    public NotesInventory notesInventory; // Assign in Inspector

    private GameObject notesMenuUI;
    private bool isMenuActive = false;

    private void Awake()
    {
        notesMenuUI = this.gameObject; 
        notesMenuUI.SetActive(false); // Initially hide the menu
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleNotesMenu();
        }
    }

    private void ToggleNotesMenu()
    {
         isMenuActive = !isMenuActive;
        notesMenuUI.SetActive(isMenuActive);

        if (isMenuActive)
        {
        PopulateNotesList();
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None; // Show cursor
        Cursor.visible = true;
        }
        else
        {
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Hide cursor
        Cursor.visible = false;
        }
    }

    private void PopulateNotesList()
    {
        // First, clear existing entries
        foreach (Transform child in notesListParent)
        {
            Destroy(child.gameObject);
        }

        // Then, populate with new entries
        foreach (Note note in notesInventory.notes)
        {
            GameObject entry = Instantiate(noteEntryPrefab, notesListParent);
            entry.GetComponentInChildren<Text>().text = note.title;
            entry.GetComponent<Button>().onClick.AddListener(() => DisplayNoteDetails(note));
        }
    }

    private void DisplayNoteDetails(Note note)
    {
        noteDescriptionText.text = $"{note.title}\n\n{note.description}";
    }
}
