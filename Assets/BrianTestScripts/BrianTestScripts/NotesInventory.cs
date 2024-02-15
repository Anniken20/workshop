using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Notes Inventory", menuName = "Notes Inventory")]
public class NotesInventory : ScriptableObject
{
    public List<Note> notes = new List<Note>();
}
