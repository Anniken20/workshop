using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fullscreentoggle : MonoBehaviour
{
    public void Change()
    {
        Screen.fullScreen = !Screen.fullScreen;
        print("Changed screen mode");
    }
}
