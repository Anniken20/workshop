using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab; // Prefab of the ghost enemy to spawn
    public float spawnCooldown = 2f; // Cooldown time between spawns
    public int maxEnemies = 5; // Maximum number of enemies to spawn
    public float spawnRadius = 1f; // Radius around spawner where enemies can spawn

    private int numEnemiesSpawned; //How many spawned total to set max
    private int numEnemiesAlive; //In case we want max enemies alive spawn
    private bool playerInsideTrigger; // Flag to track if the player is inside the trigger zone
    private Coroutine spawnCoroutine; // Reference to the spawning coroutine

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            if (spawnCoroutine == null)
                spawnCoroutine = StartCoroutine(SpawnGhosts());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = false;
        }
    }

    IEnumerator SpawnGhosts()
    {
        while (numEnemiesSpawned < maxEnemies && playerInsideTrigger)
        {
            if (numEnemiesAlive < maxEnemies)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
                numEnemiesSpawned++;
                numEnemiesAlive++;
                yield return new WaitForSeconds(spawnCooldown);
            }
            else
            {
                yield return null;
            }
        }
        spawnCoroutine = null; // Reset coroutine reference when spawning is finished
    }

    public void RegisterEnemyDestroyed()
    {
        numEnemiesAlive--;
    }
}
