using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FreeRoamerEnemy : MonoBehaviour
{
    public float roamRadius = 15f; // Radius in which the enemy can roam
    public float detectionRadius = 10f; // Radius to detect the player
    public float roamSpeed = 3.5f; // Speed of roaming
    public float chaseSpeed = 6f; // Speed when chasing the player
    public float chaseDuration = 5f; // Time before the enemy stops chasing and returns to roaming

    private Transform target; // Reference to the player
    private Vector3 startingPosition; // Starting position of the enemy
    private NavMeshAgent agent; // Reference to the NavMeshAgent
    private bool isChasing = false; // Whether the enemy is chasing the player
    private Coroutine chaseCoroutine = null; // Reference to the active chase coroutine

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        startingPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();

        // Start roaming behavior
        StartCoroutine(Roam());
    }

    void Update()
    {
        if (isChasing)
            return; // Skip roaming logic if currently chasing the player

        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (chaseCoroutine == null)
                chaseCoroutine = StartCoroutine(ChasePlayer());
        }
    }

    private IEnumerator Roam()
    {
        while (true)
        {
            // Pick a random point within the roam radius
            Vector3 roamTarget = GetRandomPointWithinRadius(startingPosition, roamRadius);

            if (roamTarget != Vector3.zero)
            {
                agent.speed = roamSpeed;
                agent.SetDestination(roamTarget);

                // Wait until the enemy reaches the destination or gets close enough
                while (!agent.pathPending && Vector3.Distance(transform.position, roamTarget) > 1f)
                {
                    if (isChasing) // Stop roaming if the enemy starts chasing
                        yield break;

                    yield return null;
                }
            }

            // Wait for a random time before picking a new roam target
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    private IEnumerator ChasePlayer()
    {
        isChasing = true;

        // Increase speed while chasing
        agent.speed = chaseSpeed;

        float elapsedTime = 0f;

        while (elapsedTime < chaseDuration)
        {
            float distanceToPlayer = Vector3.Distance(target.position, transform.position);

            // If the player escapes the detection radius, stop chasing
            if (distanceToPlayer > detectionRadius)
                break;

            // Continuously chase the player
            agent.SetDestination(target.position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to roaming after chasing
        isChasing = false;
        chaseCoroutine = null;

        StartCoroutine(Roam());
    }

    private Vector3 GetRandomPointWithinRadius(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++) // Try up to 10 times to find a valid point
        {
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            Vector3 targetPoint = new Vector3(center.x + randomPoint.x, center.y, center.z + randomPoint.y);

            NavMeshHit hit;
            // Ensure the point is on the NavMesh
            if (NavMesh.SamplePosition(targetPoint, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero; // Return zero vector if no valid point is found
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startingPosition, roamRadius); // Roaming area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Detection area
    }
}
