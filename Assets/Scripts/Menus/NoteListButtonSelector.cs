using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteListButtonSelector : MonoBehaviour
{
    //public GameObject button;
    public void FindNotesInChildren(){
        foreach(Transform child in transform){
            if(child.gameObject!= null){
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(child.gameObject);
                //FindObjectOfType<MenuManager>().currentHovered = child.gameObject;
                break;
            }
            else{
                break;
            }
        }
    }
}
