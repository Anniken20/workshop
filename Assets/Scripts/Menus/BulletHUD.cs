using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void DrawBullets()
    {
        currentAmmo = totalAmmo;
        singleBullet = transform.GetChild(0).gameObject;
        bulletsArray = new GameObject[totalAmmo];
        bulletsArray[0] = singleBullet;

        for (int i = 1; i < totalAmmo; ++i)
        {
            GameObject sb = Instantiate(singleBullet, singleBullet.transform);
            sb.transform.localPosition += new Vector3(offset * i, 0);
            bulletsArray[i] = sb;
        }
    }

    public void StartBulletHUD(int totalAmmo)
    {
        this.totalAmmo = totalAmmo;
        DrawBullets();
    }

    public void SubtractBulletHUD()
    {
        currentAmmo--;
        bulletsArray[currentAmmo].SetActive(false);
    }

    public void AddBulletHUD()
    {
        bulletsArray[currentAmmo].SetActive(true);
        currentAmmo++;
    }

}
