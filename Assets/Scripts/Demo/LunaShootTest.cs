using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaShootTest : MonoBehaviour, ILunaShootable
{
    public void OnLunaShot(BulletController bulletController)
    {
        Destroy(gameObject, 0.2f);
    }
}
