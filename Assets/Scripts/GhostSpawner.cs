using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab; // Prefab of the ghost enemy to spawn
    public float spawnCooldown = 2f; // Cooldown time between spawns
    public int maxEnemies = 5; // Maximum number of enemies to spawn
    public float spawnRadius = 1f; // Radius around spawner where enemies can spawn

    public Transform spawnPoint; // Reference to the spawn point GameObject
    public float spawnHeight = 1f; // Height at which the ghosts will spawn

    private int numEnemiesSpawned; // How many spawned total to set max
    private int numEnemiesAlive; // In case we want max enemies alive spawn
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
                // Randomize X and Z coordinates within spawn radius, keep Y-coordinate constant
                Vector3 spawnPosition = spawnPoint.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), spawnHeight, Random.Range(-spawnRadius, spawnRadius));
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

    public void ForceSpawns()
    {
        playerInsideTrigger = true;
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnGhosts());
    }

    public void ForceStopSpawns()
    {
        playerInsideTrigger = false;
        if (spawnCoroutine == null)
        {
            StopCoroutine(SpawnGhosts());
            spawnCoroutine = null;
        }
    }
}
