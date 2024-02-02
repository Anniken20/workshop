using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShotDemo : MonoBehaviour, IShootable
{
    public void OnShot(BulletController bullet)
    {
        TakeDamage((int)bullet.currDmg);
        Debug.Log("hehe");
    }

    private void TakeDamage(int x)
    {
        Debug.Log("I took " + x + " damage!");
    }
}
