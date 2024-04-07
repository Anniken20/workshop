using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintTextController : MonoBehaviour
{
    public string textBox;
    [SerializeField] InventoryManager.AllItems requiredItem; 

    // Start is called before the first frame update
    public void FillText()
    {
        if(!HasRequiredItem(requiredItem))
        {
            HintTextSingleton.Main.SetHintMessage(textBox);
        }
        else
        {
            HintTextSingleton.Main.SetHintMessage("");
        }
    }
    public void HideText()
    {
        HintTextSingleton.Main.HideMessage();
    }

    public bool HasRequiredItem(InventoryManager.AllItems itemRequired)
    {
        if (InventoryManager.Instance.inventoryItems.Contains(itemRequired))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
