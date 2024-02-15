using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleNote : MonoBehaviour
{
    public Note note;
    public NotesInventory playerNotesInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            CollectNote();
            gameObject.SetActive(false); // Optionally disable the collectible
        }
    }

    private void CollectNote()
    {
        if (!playerNotesInventory.notes.Contains(note))
        {
            playerNotesInventory.notes.Add(note);
            Debug.Log($"Collected note added: {note.title}");
            // Optionally, trigger some UI feedback here
        }
    }
}
