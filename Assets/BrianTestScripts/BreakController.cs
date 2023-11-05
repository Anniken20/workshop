using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
    public GameObject[] fragments; // An array to store the smaller fragments in the Inspector
    public float explosionForce = 10f;
    public float explosionRadius = 5f;
    public float disableRigidbodyDelay = 30f; // Time before disabling rigidbodies
    public float enableRigidbodyDelay = 30f; // Time to enable rigidbodies again if not motionless

    private Rigidbody[] fragmentRigidbodies;
    private Vector3[] initialFragmentPositions;
    private bool rigidbodiesDisabled = false;
    private float lastMovementTime;

    private void Start()
    {
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

            // Destroy the original object
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Fragments not assigned in the Inspector.");
        }
    }
}
