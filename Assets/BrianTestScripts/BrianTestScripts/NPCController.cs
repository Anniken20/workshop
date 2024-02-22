using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float patrolTime = 10f; // Time between new patrol points
    public float lookRadius = 10f; // Detection radius for player
    private float nextDestinationTime = 0f;
    private Transform target; // Player's transform
    private NavMeshAgent agent;
    private Vector3 originalPosition;

    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalPosition = transform.position; // Remember start position for patrolling
        target = GameObject.FindWithTag("Player").transform; // Find player by tag
        StartCoroutine(UpdatePath()); // Start the path updating routine
    }


    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            FaceTarget(); // Turn to face player if within look radius
        }
    }

     void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.5f;

        while (target != null)
        {
            if (Time.time >= nextDestinationTime)
            {
                Vector3 newDestination = originalPosition + Random.insideUnitSphere * patrolTime; // Random point within patrol radius
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newDestination, out hit, patrolTime, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
                nextDestinationTime = Time.time + 30f; // Update only every 30 seconds
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    // Utilize this to draw the look radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
