using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
     public GameObject[] fragments; // An array to store the smaller fragments in the Inspector
    public float explosionForce = 10f;
    public float explosionRadius = 5f;

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
