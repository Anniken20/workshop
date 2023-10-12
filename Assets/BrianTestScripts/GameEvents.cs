using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public int keys {get; private set; }
    

    public void KeyCount()
    {
        keys++;
    }

}
