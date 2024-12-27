using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PufferfishController : MonoBehaviour
{
    public float detectionRadius = 5f;  // Radius within which the pufferfish puffs up
    public float pushForce = 10f;      // Force applied to the player
    public float puffDuration = 2f;   // Time the pufferfish stays puffed up
    public float cooldownTime = 3f;   // Time before the pufferfish can puff up again
    public float puffSpeed = 5f;      // Speed at which the pufferfish puffs up

    private bool isPuffed = false;    // Whether the pufferfish is currently puffed up
    private bool onCooldown = false; // Whether the pufferfish is on cooldown
    private Vector3 originalScale;   // Original size of the pufferfish
    private Vector3 puffedScale;     // Size of the pufferfish when puffed
    public Transform player;         // Reference to the player's Transform
    private PatrolController patrolController; // Reference to the PatrolController script

    void Start()
    {
        // Save the original scale of the pufferfish and calculate the puffed scale
        originalScale = transform.localScale;
        puffedScale = originalScale * 2f;

        // Find the player if not already assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Get the PatrolController script if it's attached to the same GameObject
        patrolController = GetComponent<PatrolController>();
    }

    void Update()
    {
        if (!isPuffed && !onCooldown)
        {
            // Check if the player is within the detection radius
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                // Stop patrolling and puff up
                if (patrolController != null)
                {
                    patrolController.StopPatrolling();
                }

                StartCoroutine(PuffUp());
            }
        }
    }

    private IEnumerator PuffUp()
    {
        isPuffed = true;

        // Smoothly scale up to the puffed size
        float elapsedTime = 0f;
        while (elapsedTime < (1f / puffSpeed))
        {
            transform.localScale = Vector3.Lerp(originalScale, puffedScale, elapsedTime * puffSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = puffedScale;

        // Apply a force to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // Apply force only if there's distance between them
            if (Vector3.Distance(transform.position, player.position) > 0.1f)
            {
                playerRb.AddForce(directionToPlayer * pushForce, ForceMode.Impulse);
            }
        }

        // Wait for the puff duration
        yield return new WaitForSeconds(puffDuration);

        // Smoothly scale down to the original size
        elapsedTime = 0f;
        while (elapsedTime < (1f / puffSpeed))
        {
            transform.localScale = Vector3.Lerp(puffedScale, originalScale, elapsedTime * puffSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;

        // Enter cooldown phase
        isPuffed = false;
        onCooldown = true;

        // Wait for the cooldown time
        yield return new WaitForSeconds(cooldownTime);

        // Cooldown complete
        onCooldown = false;

        // Resume patrolling after cooldown
        if (patrolController != null)
        {
            patrolController.ResumePatrolling();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
