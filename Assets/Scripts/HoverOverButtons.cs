using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverButtons : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler, IPointerClickHandler
{
    public GameObject hoverPanel; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ScaleDown();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ScaleDown();
    }

    public void ScaleUp()
    {
        hoverPanel.transform.localScale *= 1.2f;
    }

    public void ScaleDown()
    {
        hoverPanel.transform.localScale /= 1.2f;
    }
}
