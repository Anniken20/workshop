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
    public float minDistanceBetweenGhosts = 2f; // Minimum distance between spawn positions

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
        Debug.Log("Should Spawn Ghost");
        while (numEnemiesSpawned < maxEnemies && playerInsideTrigger)
        {
            if (numEnemiesAlive < maxEnemies)
            {
                // Get all potential spawn positions within the spawn radius
                List<Vector3> potentialSpawnPositions = GetPotentialSpawnPositions();

                // Check if there are potential spawn positions
                if (potentialSpawnPositions.Count > 0)
                {
                    // Randomly select one of the potential spawn positions
                    Vector3 spawnPosition = potentialSpawnPositions[Random.Range(0, potentialSpawnPositions.Count)];

                    // Instantiate the ghost at the selected spawn position
                    Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
                    numEnemiesSpawned++;
                    numEnemiesAlive++;

                    yield return new WaitForSeconds(spawnCooldown);
                }
                else
                {
                    // If no suitable spawn positions are found, wait and try again
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
        spawnCoroutine = null; // Reset coroutine reference when spawning is finished
    }

    List<Vector3> GetPotentialSpawnPositions()
    {
        List<Vector3> potentialSpawnPositions = new List<Vector3>();

        // Generate random points within the spawn area and check if they are too close to existing ghosts
        int maxAttempts = 10; // Maximum number of attempts to find a suitable spawn position
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPoint = spawnPoint.position + new Vector3(Random.Range(0f, 2f), spawnHeight, Random.Range(0f, 2f));

            if (!IsSpawnPositionTooClose(randomPoint))
            {
                potentialSpawnPositions.Add(randomPoint);
            }
        }

        return potentialSpawnPositions;
    }

    bool IsSpawnPositionTooClose(Vector3 spawnPosition)
    {
        // Check if the spawn position is too close to any existing spawn positions
        foreach (GameObject ghost in GameObject.FindGameObjectsWithTag("Ghost"))
        {
            if (Vector3.Distance(ghost.transform.position, spawnPosition) < minDistanceBetweenGhosts)
            {
                return true;
            }
        }
        return false;
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
