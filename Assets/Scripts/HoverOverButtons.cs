using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverButtons : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    public GameObject hoverPanel; 

    public void Start ()
    {
    }

   public void OnPointerEnter(PointerEventData eventData)
   {

    hoverPanel.transform.localScale *= 1.2f;

    //hoverPanel.SetActive(true);

// throw new System.NotImplementedException();

   }

    public void OnPointerExit(PointerEventData eventData)
   {

   hoverPanel.transform.localScale /= 1.2f;

   // hoverPanel.SetActive(false);
// throw new System.NotImplementedException();

   }

}
