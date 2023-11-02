using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] bossPrefabs; // A list for all bosses
    public int bossToSpawnIndex = 0; // which one to spawn here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the trigger, boss in list spawns
            SpawnSpecificBoss();
        }
    }

    private void SpawnSpecificBoss()
    {
        // Check if boss can spawn
        if (bossToSpawnIndex < 0 || bossToSpawnIndex >= bossPrefabs.Length)
        {
            Debug.LogWarning("Hey it doesn't fucking work.");
            return;
        }

        // Spawn the specified boss
        GameObject bossPrefab = bossPrefabs[bossToSpawnIndex];
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }
}