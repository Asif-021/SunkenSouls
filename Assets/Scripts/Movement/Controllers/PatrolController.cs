using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class PatrolController : MonoBehaviour
{
    public List<Transform> patrolPoints; // List of patrol points
    public float waitTimeAtPoint = 1f;   // Time spent waiting at each point

    private NavMeshAgent navMeshAgent;  // Reference to the NavMeshAgent component
    private int currentPatrolIndex = 0; // Current patrol point index
    private bool isWaiting = false;    // State to check if waiting

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing from this GameObject.");
            return;
        }

        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned to PatrolController.");
            return;
        }

        // Move to the first patrol point
        SetDestination(patrolPoints[currentPatrolIndex]);
    }

    void Update()
    {
        if (!isWaiting && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        // Wait for the specified time
        yield return new WaitForSeconds(waitTimeAtPoint);

        // Move to the next patrol point
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        SetDestination(patrolPoints[currentPatrolIndex]);

        isWaiting = false;
    }

    private void SetDestination(Transform target)
    {
        if (navMeshAgent != null && target != null)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }

    // Stop the patrolling behavior
    public void StopPatrolling()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }
    }

    // Resume the patrolling behavior
    public void ResumePatrolling()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
            SetDestination(patrolPoints[currentPatrolIndex]);
        }
    }
}
