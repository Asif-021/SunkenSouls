using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    public List<Transform> patrolPoints; // List of patrol points
    public float patrolSpeed = 3f;       // Speed of movement
    public float waitTimeAtPoint = 1f;   // Time spent waiting at each point

    private int currentPatrolIndex = 0;  // Current patrol point index
    private bool isPatrolling = true;   // State to check if patrolling
    private bool isWaiting = false;     // State to check if waiting

    void Start()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned to PatrolController.");
            return;
        }

        // Move to the first patrol point
        transform.position = patrolPoints[currentPatrolIndex].position;
    }

    void Update()
    {
        if (isPatrolling && !isWaiting)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];

        // Move towards the current patrol point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        // Check if the object has reached the patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
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
        isWaiting = false;
    }

    // Stop the patrolling behavior
    public void StopPatrolling()
    {
        isPatrolling = false;
    }

    // Resume the patrolling behavior
    public void ResumePatrolling()
    {
        isPatrolling = true;
    }
}
