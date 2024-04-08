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

    private void ClearBullets()
    {
        foreach (var bullet in bulletsList)
        {
            Destroy(bullet);
        }
        bulletsList.Clear();
    }

    public void UpdateBulletHUD(int currentAmmo)
    {
        ClearBullets(); // Remove all current bullets from the HUD

        if (bulletsList.Count == 0)
        {
            // Ensure the prototype bullet is set up
            singleBullet = transform.GetChild(0).gameObject;
            singleBullet.SetActive(false); // Hide the prototype after capturing it
        }

        for (int i = 0; i < currentAmmo; ++i)
        {
            // Instantiate and position each bullet
            GameObject sb = Instantiate(singleBullet, transform, false);
            sb.SetActive(true); // Make sure new bullets are visible
            sb.transform.localPosition = new Vector3(offset * i, 0, 0);
            bulletsList.Add(sb);
        }
    }

    public void StartBulletHUD(int ammo)
    {
        UpdateBulletHUD(ammo);
    }

    public void SubtractBulletHUD()
    {
        if (bulletsList.Count > 0)
        {
        GameObject lastBullet = bulletsList[bulletsList.Count - 1];
        bulletsList.RemoveAt(bulletsList.Count - 1);
        Destroy(lastBullet);
        }
    }

    public void AddBulletHUD()
    {
        if (currentAmmo < totalAmmo)
        {
            bulletsList[currentAmmo].SetActive(true);
            currentAmmo++;
        }
        else 
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