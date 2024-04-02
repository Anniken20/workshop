using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintTextController : MonoBehaviour
{
    public string textBox;

    // Start is called before the first frame update
    public void FillText()
    {
        HintTextSingleton.Main.SetHintMessage(textBox); 
    }
    public void HideText()
    {
        HintTextSingleton.Main.HideMessage();
    }



    
}
