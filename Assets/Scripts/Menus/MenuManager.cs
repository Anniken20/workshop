using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject firstMenu;
    [SerializeField] GameObject secondMenu;

    [SerializeField] GameObject oneFirstSelected;
    [SerializeField] GameObject twoFirstSelected;
    private GameObject currentHovered;
    private GameObject previousHovered;
    private bool doOnce;

    private void Awake(){
        MenuOneOpen();
        previousHovered = oneFirstSelected;
        currentHovered = oneFirstSelected;
        HoverStart(oneFirstSelected);
    }
    private void Update(){
        if(firstMenu.activeSelf == false && secondMenu.activeSelf == false){
            EventSystem.current.SetSelectedGameObject(null);
            currentHovered = null;
        }
        if(currentHovered != EventSystem.current.currentSelectedGameObject){
            if(doOnce){
                currentHovered = EventSystem.current.currentSelectedGameObject;
                HoverEnd(previousHovered);
                HoverStart(currentHovered);
                doOnce = false;
            }
        }
        else{
            doOnce = true;
            previousHovered = currentHovered;
        }

    }
    public void MenuOneOpen(){
        EventSystem.current.SetSelectedGameObject(oneFirstSelected);
        currentHovered = oneFirstSelected;
    }
    public void MenuTwoOpen(){
         EventSystem.current.SetSelectedGameObject(twoFirstSelected);
    }

    private void HoverStart(GameObject button){
        if(button != null){
            button.transform.localScale *= 1.2f;
        }
    }
    private void HoverEnd(GameObject button){
        if(button != null){
            button.transform.localScale /= 1.2f;
        }
    }
}
