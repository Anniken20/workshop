using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
    public GameObject[] fragments; // An array to store the smaller fragments in the Inspector
    public GameObject coinPrefab;
    public int minCoins = 1;
    public int maxCoins = 5;
    public float coinForce = 5f;
    public float explosionForce = 10f;
    public float explosionRadius = 5f;
    public float disableRigidbodyDelay = 30f; // Time before disabling rigidbodies
    public float enableRigidbodyDelay = 30f; // Time to enable rigidbodies again if not motionless
    public AudioClip breakSound;

    private Rigidbody[] fragmentRigidbodies;
    private Vector3[] initialFragmentPositions;
    private bool rigidbodiesDisabled = false;
    private float lastMovementTime;
    private AudioSource audioSource;

    private void Start()
    {
         audioSource = GetComponent<AudioSource>();
        // Check if there are fragments assigned
        if (fragments != null && fragments.Length > 0)
        {
            // Initialize arrays and capture initial positions
            fragmentRigidbodies = new Rigidbody[fragments.Length];
            initialFragmentPositions = new Vector3[fragments.Length];

            for (int i = 0; i < fragments.Length; i++)
            {
                GameObject fragment = fragments[i];
                fragmentRigidbodies[i] = fragment.GetComponent<Rigidbody>();
                initialFragmentPositions[i] = fragment.transform.position;
            }
        }
        else
        {
            Debug.LogError("Fragments not assigned in the Inspector.");
        }
    }

    private void Update()
    {
        if (!rigidbodiesDisabled)
        {
            bool fragmentsAreMotionless = true;

            // Check if any fragment has moved
            for (int i = 0; i < fragmentRigidbodies.Length; i++)
            {
                if (fragmentRigidbodies[i] != null && fragmentRigidbodies[i].IsSleeping())
                {
                    lastMovementTime = Time.time;
                }
                else
                {
                    fragmentsAreMotionless = false;
                }
            }

            if (fragmentsAreMotionless && Time.time - lastMovementTime > disableRigidbodyDelay)
            {
                // Disable rigidbodies after 30 seconds of inactivity
                for (int i = 0; i < fragmentRigidbodies.Length; i++)
                {
                    if (fragmentRigidbodies[i] != null)
                    {
                        fragmentRigidbodies[i].isKinematic = true;
                    }
                }
                rigidbodiesDisabled = true;
            }
        }
        else if (Time.time - lastMovementTime <= enableRigidbodyDelay)
        {
            // If fragments become active within 30 seconds after disabling, re-enable rigidbodies
            for (int i = 0; i < fragmentRigidbodies.Length; i++)
            {
                if (fragmentRigidbodies[i] != null)
                {
                    fragmentRigidbodies[i].isKinematic = false;
                }
            }
            rigidbodiesDisabled = false;
        }
    }

    public void BreakIntoPieces()
    {
        // Check if there are fragments assigned
        if (fragments != null && fragments.Length > 0)
        {
            // Calculate the position for the smaller fragments
            Vector3 originalPosition = transform.position;
            Vector3 fragmentSize = transform.localScale / 3; // Assuming the box is divided into 9 smaller boxes

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        Vector3 fragmentPosition = originalPosition + new Vector3(x * fragmentSize.x, y * fragmentSize.y, z * fragmentSize.z);

                        // Instantiate the fragment
                        GameObject fragment = Instantiate(fragments[Random.Range(0, fragments.Length)], fragmentPosition, Quaternion.identity);

                        // Apply an explosion force to the fragment
                        Rigidbody rb = fragment.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Vector3 randomDirection = Random.onUnitSphere;
                            rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);
                        }
                    }
                }
            }
            PlayBreakSound();

            int coinsToSpawn = Random.Range(minCoins, maxCoins + 1);
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnCoin();
            }

            // Destroy the original object
            Destroy(gameObject);
            
        }
        else
        {
            Debug.LogError("Fragments not assigned in the Inspector.");
        }
    }

    private void SpawnCoin()
{
    if (coinPrefab != null)
    {
        Vector3 spawnPosition = GetValidSpawnPosition();
        if (spawnPosition != Vector3.zero) // Check if a valid position was found
        {
            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            Rigidbody coinRb = coin.GetComponent<Rigidbody>();
            if (coinRb != null)
            {
                // Apply a force in a random direction
                Vector3 forceDirection = Random.onUnitSphere;
                coinRb.AddForce(forceDirection * coinForce, ForceMode.Impulse);
            }
        }
    }
    else
    {
        Debug.LogError("Coin prefab not set.");
    }
}

private Vector3 GetValidSpawnPosition()
{
    int attempts = 20; // Increase the number of attempts
    float spawnRadius = 0.5f; // Reduce the radius for checking clear space
    float spawnDistance = 5f; // Distance from the object center to attempt spawning

    for (int i = 0; i < attempts; i++)
    {
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnOffset = randomDirection * spawnDistance;
        spawnOffset.y = Mathf.Abs(spawnOffset.y); // Ensure the y offset is positive
        Vector3 spawnPosition = transform.position + spawnOffset;

        // Check if the position is clear of other colliders
        if (!Physics.CheckSphere(spawnPosition, spawnRadius))
        {
            Debug.Log("Valid spawn position found: " + spawnPosition); // Add a debug statement
            return spawnPosition;
        }
    }

    Debug.LogWarning("No valid spawn position found, using fallback."); // Fallback warning
    return transform.position; // Fallback to the object's position
}

    private void PlayBreakSound()
    {
        if (audioSource != null && breakSound != null)
        {
            audioSource.PlayOneShot(breakSound); 
        }
        else
        {
            Debug.LogError("AudioSource or breakSound not set.");
        }
    }
}
