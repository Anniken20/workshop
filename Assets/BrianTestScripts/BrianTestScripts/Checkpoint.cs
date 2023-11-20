using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static List<Checkpoint> checkpoints = new List<Checkpoint>();
    private static Checkpoint activeCheckpoint;

    private void Start()
    {
        checkpoints.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Set this checkpoint as the active checkpoint
            activeCheckpoint = this;
        }
    }

    public static Checkpoint GetActiveCheckpoint()
    {
        return activeCheckpoint;
    }

    public static List<Checkpoint> GetCheckpoints()
    {
        return checkpoints;
    }
}


