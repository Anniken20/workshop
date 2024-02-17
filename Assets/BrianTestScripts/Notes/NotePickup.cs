using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : Pickup
{
    [SerializeField] Note note = null;

    [SerializeField] bool autoDisplay = false;
    [SerializeField] bool add = true;

    protected override void PickupAction()
    {
        base.PickupAction();
        if (autoDisplay)
        {
            NotesSystem.Display(note);
        }
        if (add)
        {
            NotesSystem.AddNote(note.Label, note);
        }
    }
}
