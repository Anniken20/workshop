using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectedButtonOnWake : MonoBehaviour
{
    public GameObject button;
    private void Start(){
        
    }
    public void OnEnable(){
        Debug.Log("Im enabled");
        StartCoroutine(Wait());
    }
    private IEnumerator Wait(){
        yield return new WaitForSecondsRealtime(0.01f);
        Debug.Log("I am actually doing something");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
        FindObjectOfType<MenuManager>().currentHovered = button;
    }
    private void OnDisable(){
        EventSystem.current.SetSelectedGameObject(null);
    }
}
