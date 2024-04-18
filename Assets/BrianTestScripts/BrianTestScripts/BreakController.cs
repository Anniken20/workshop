using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public float clipLength;
    public int maxFragmentsToSpawn = 5;
    [SerializeField] private float spawnDistance = 3f;

    private Rigidbody[] fragmentRigidbodies;
    private Vector3[] initialFragmentPositions;
    private bool rigidbodiesDisabled = false;
    private float lastMovementTime;
    private AudioSource audioSource;
    private Collider c;

    public Dissolver dissolver;
    public bool deadEnemy = false;

    private void Start()
    {
         audioSource = GetComponent<AudioSource>();
        c = GetComponent<Collider>();
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

        dissolver = GetComponent<Dissolver>();
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
        StartCoroutine(BreakWithSFX(clipLength));
        dissolver.InitAndDissolve();

        //Debug.Log("BreakIntoPieces and dissolve called called.");

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
    float spawnRadius = 0.4f; // Reduce the radius for checking clear space
   

    for (int i = 0; i < attempts; i++)
    {
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 spawnOffset = randomDirection * spawnDistance;
        spawnOffset.y = Mathf.Abs(spawnOffset.y); // Ensure the y offset is positive
        Vector3 spawnPosition = transform.position + spawnOffset;

        // Check if the position is clear of other colliders
        if (!Physics.CheckSphere(spawnPosition, spawnRadius))
        {
            //Debug.Log("Valid spawn position found: " + spawnPosition); // Add a debug statement
            return spawnPosition;
        }
    }

    //Debug.LogWarning("No valid spawn position found, using fallback."); // Fallback warning
    return transform.position; // Fallback to the object's position
}

    private void PlayBreakSound()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        
        if(meshRenderer != null)
        {
            meshRenderer.enabled = false;
            c.enabled = false;
            if (audioSource != null && breakSound != null)
            {
                audioSource.PlayOneShot(breakSound);
            }
            else
            {
                Debug.LogError("AudioSource or breakSound not set.");
            }
        }
        else
        {
            return;
        }

    }

    IEnumerator BreakWithSFX(float clipLength)
    {
        if (fragments != null && fragments.Length > 0)
        {
            Vector3 originalPosition = transform.position;
            // Use Mathf.Min to ensure we don't attempt to spawn more fragments than we have available or exceed our maxFragmentsToSpawn
            int fragmentsToSpawn = Mathf.Min(maxFragmentsToSpawn, fragments.Length);

            Debug.Log($"Spawning {fragmentsToSpawn} fragments.");

        for (int i = 0; i < fragmentsToSpawn; i++)
        {
            // Randomize position around the original object within a specified range
            Vector3 spawnOffset = Random.insideUnitSphere * explosionRadius; // Adjust radius as needed
            Vector3 fragmentPosition = originalPosition + spawnOffset;

            // Choose a random fragment from the available prefabs
            GameObject fragmentPrefab = fragments[Random.Range(0, fragments.Length)];
            GameObject fragment = Instantiate(fragmentPrefab, fragmentPosition, Quaternion.identity);
            Debug.Log($"Spawned fragment at {fragmentPosition}.");

            // Apply an explosion force to the fragment
            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);
            }
        }
        
            int coinsToSpawn = Random.Range(minCoins, maxCoins + 1);
            for (int i = 0; i < coinsToSpawn; i++)
            {
                SpawnCoin();
            }


            PlayBreakSound();
            yield return new WaitForSeconds(clipLength);

            // Destroy the original object
            if(!deadEnemy) Destroy(gameObject);
            else
            {
                transform.DOMoveY(transform.position.y - 5f, 15f);
            }
        }
        else
        {
            Debug.LogError("Fragments not assigned in the Inspector.");
        }
            yield return null;
    }
}
