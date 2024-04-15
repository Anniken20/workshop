using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractPopup : MonoBehaviour
{
    public static InteractPopup Main;
    public static TextMeshProUGUI textMesh;
    private void Awake()
    {
        Main = this;
        textMesh = GetComponent<TextMeshProUGUI>();
        if (textMesh == null) Debug.Log("No text mesh set on interact popup");
        //else Debug.Log("Interact popup set");
        gameObject.SetActive(false);
    }
}
