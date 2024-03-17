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
    private List<GameObject> bulletsList = new List<GameObject>();

    public void DrawBullets()
    {
        currentAmmo = totalAmmo;
        singleBullet = transform.GetChild(0).gameObject;
        singleBullet.SetActive(true); // Ensure the initial bullet is active.

        bulletsList.Clear(); // Clear the list before redrawing.
        bulletsList.Add(singleBullet); // Add the first bullet to the list.

        for (int i = 1; i < totalAmmo; ++i)
        {
            GameObject sb = Instantiate(singleBullet, transform, false);
            sb.transform.localPosition = singleBullet.transform.localPosition + new Vector3(offset * i, 0, 0);
            bulletsList.Add(sb);
        }
    }

    public void StartBulletHUD(int ammo)
    {
        totalAmmo = ammo;
        DrawBullets();
    }

    public void SubtractBulletHUD()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            bulletsList[currentAmmo].SetActive(false);
        }
    }

    public void AddBulletHUD()
    {
        if (currentAmmo < totalAmmo)
        {
            bulletsList[currentAmmo].SetActive(true);
            currentAmmo++;
        }
        else if (currentAmmo == totalAmmo)
        {
            GameObject newBullet = Instantiate(singleBullet, transform, false);
            newBullet.transform.localPosition = singleBullet.transform.localPosition + new Vector3(offset * totalAmmo, 0, 0);
            newBullet.SetActive(true);
            bulletsList.Add(newBullet);
            currentAmmo++;
            totalAmmo++;
        }
    }
}