using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour
{
   public GameObject smallPiecePrefab;
    public int numberOfPieces = 10;
    public float explosionForce = 10f;
    public float explosionRadius = 5f;

    public void BreakIntoPieces()
    {
        // Create small pieces
        for (int i = 0; i < numberOfPieces; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            GameObject smallPiece = Instantiate(smallPiecePrefab, transform.position, Quaternion.identity);
            Rigidbody rb = smallPiece.GetComponent<Rigidbody>();

            // Apply force to the small pieces
            rb.AddForce(randomDirection * explosionForce, ForceMode.Impulse);
        }

        // Destroy the original object
        Destroy(gameObject);
    }
}
