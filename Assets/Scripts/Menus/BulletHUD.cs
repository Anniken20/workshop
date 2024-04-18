using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Script for the bullets on HUD. Referenced by GunController. 
 * Works dynamically depending on bullet count 8)
 * 
 * Caden Henderson
 * 12/2/2023
 */

public class BulletHUD : MonoBehaviour
{
    private GameObject singleBullet;
    private float offset = 25f;
    private int totalAmmo;
    private int currentAmmo;
    private GameObject[] bulletsArray;


    private void DrawBullets()
    {
        currentAmmo = totalAmmo;
        singleBullet = transform.GetChild(0).gameObject;
        bulletsArray = new GameObject[totalAmmo];
        bulletsArray[0] = singleBullet;

        for (int i = 1; i < totalAmmo; ++i)
        {
            GameObject sb = Instantiate(singleBullet, singleBullet.transform.parent);
            sb.transform.localPosition += new Vector3(offset * i, 0);
            bulletsArray[i] = sb;
        }
    }

    public void SubtractBulletHUD()
    {
        currentAmmo--;
        bulletsArray[currentAmmo].SetActive(false);
    }

    public void AddBulletHUD()
    {
        if(currentAmmo >= bulletsArray.Length)
        {
            UpdateBulletHUD(currentAmmo);
        } else
        {
            bulletsArray[currentAmmo].SetActive(true);
            currentAmmo++;
        }
    }

    public void UpdateBulletHUD(int newAmmo)
    {
        if (newAmmo != totalAmmo)
        {
            ClearBullets();
            totalAmmo = newAmmo;
            DrawBullets();
        }
    }

    private void ClearBullets()
    {
        for (int i = 1; i < totalAmmo; ++i)
        {
            if (bulletsArray[i] == null) return;
            Destroy(bulletsArray[i]);
        }
        Array.Resize(ref bulletsArray, 1);
    }
}