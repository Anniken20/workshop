using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverButtons : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler, IPointerClickHandler
{
    public GameObject hoverPanel;
    private Vector3 defaultLocalScale;
    private bool firstEnable = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //ScaleDown();
        hoverPanel.transform.localScale = defaultLocalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //ScaleDown();
        hoverPanel.transform.localScale = defaultLocalScale;
    }

    private void ScaleUp()
    {
        hoverPanel.transform.localScale *= 1.2f;
    }

    private void ScaleDown()
    {
        hoverPanel.transform.localScale /= 1.2f;
    }

    private void OnEnable()
    {
        if (firstEnable)
        {
            firstEnable = false;
            defaultLocalScale = hoverPanel.transform.localScale;
        }
    }

    private void OnDisable()
    {
        hoverPanel.transform.localScale = defaultLocalScale;
    }
}
