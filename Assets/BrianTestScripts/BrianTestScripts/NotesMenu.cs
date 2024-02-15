using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesMenu : MonoBehaviour
{
public GameObject noteMenuPanel; // Assign in inspector
    public GameObject noteButtonPrefab; // Assign a prefab with a Button and TextMeshPro component for the title
    public Transform notesListParent; // Assign the content of the ScrollView
    public TextMeshProUGUI descriptionText; // Change type to TextMeshProUGUI, assign in inspector
    public NotesInventory notesInventory; // Assign in inspector

    void Start()
    {
    noteMenuPanel.SetActive(false); // This hides the notes menu at game start
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
        bool isActive = noteMenuPanel.activeSelf;
        noteMenuPanel.SetActive(!isActive);
        Time.timeScale = isActive ? 1 : 0; // Resume time if menu is closing, pause if opening
        Cursor.visible = !isActive; // Show cursor when menu is active
        Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None; // Unlock cursor when menu is active

        if (!isActive)
        {
            PopulateNotesList();
        }
    }

    private void PopulateNotesList()
    {
        // Clear existing buttons
        foreach (Transform child in notesListParent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log($"Populating notes list. Total notes: {notesInventory.notes.Count}");

        // Create new buttons for each note
        foreach (Note note in notesInventory.notes)
        {
            GameObject buttonObj = Instantiate(noteButtonPrefab, notesListParent);
            TextMeshProUGUI textComponent = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = note.title;
            Button btn = buttonObj.GetComponent<Button>();
            btn.onClick.AddListener(delegate { OnNoteSelected(note); });
            Debug.Log($"Adding button for note: {note.title}");
        }
    }

    private void OnNoteSelected(Note note)
    {
    descriptionText.text = $"{note.title}\n\n{note.description}";
    }
}
