using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaShootTest2 : MonoBehaviour, ILunaShootable
{
    public void OnLunaShot(BulletController bullet)
    {
        Debug.Log("test test");
    }
}
