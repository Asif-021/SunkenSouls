using UnityEngine;
using UnityEngine.AI;

/* Makes enemies follow and attack the player */
public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public GameObject player; // Public GameObject for the player reference

    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player reference is not assigned in the EnemyController script!");
        }

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null)
            return;

        // Get the distance to the player
        float distance = Vector3.Distance(target.position, transform.position);

        // If inside the radius
        if (distance <= lookRadius)
        {
            // Move towards the player
            agent.SetDestination(target.position);
        }
    }

    // Point towards the player
    void FaceTarget()
    {
        if (target == null)
            return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
