using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class AmbusherEnemy : MonoBehaviour
{
    public float detectionRadius = 10f; // Radius to detect the player
    public float attackSpeed = 20f;     // Speed of the ambush
    public float cooldownTime = 2f;    // Cooldown after attacking
    public float ambushDistance = 5f;  // Distance the ambusher lunges forward (adjustable)
    public LayerMask obstructionMask;  // Mask for obstacles that block vision

    private Transform target;          // Reference to the player
    private Vector3 originalPosition;  // The ambusher's starting position
    private bool isAttacking = false;  // Whether the ambusher is in attack mode
    private bool isOnCooldown = false; // Whether the ambusher is on cooldown

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        originalPosition = transform.position; // Store the starting position
    }

    void Update()
    {
        if (isAttacking || isOnCooldown)
            return; // Skip updates if attacking or on cooldown

        // Check distance to the player
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= detectionRadius && IsPlayerInSight())
        {
            StartCoroutine(Ambush());
        }
    }

    private bool IsPlayerInSight()
    {
        // Check if the player is in direct line of sight
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
        {
            if (hit.transform == target)
            {
                return true; // Player is in sight
            }
        }

        return false; // Player is not visible
    }

    private IEnumerator Ambush()
    {
        isAttacking = true;

        // Calculate direction and move quickly towards the player
        Vector3 attackDirection = (target.position - transform.position).normalized;
        Vector3 attackTarget = transform.position + attackDirection * ambushDistance; // Use the public ambushDistance variable

        float attackTime = 0.5f; // Duration of the attack
        float elapsedTime = 0f;

        while (elapsedTime < attackTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackTarget, attackSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Teleport back to the original position
        transform.position = originalPosition;

        // Wait for cooldown before allowing another attack
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);

        isAttacking = false; // Reset attacking state
        isOnCooldown = false; // Reset cooldown state
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}