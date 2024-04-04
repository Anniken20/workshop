using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFontLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FontManager fMan = FindObjectOfType<FontManager>();
        fMan.UpdateAllTextUI();
    }
}
