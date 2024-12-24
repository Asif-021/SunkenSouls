using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveFishController : MonoBehaviour
{
    public float detectionRadius = 5f;    // Radius within which the fish explodes
    public float explosionForce = 20f;   // Force applied to nearby objects
    public float explosionRadius = 10f;  // Radius of the explosion effect
    public GameObject explosionEffectPrefab; // Prefab for the explosion effect
    public Transform player;             // Reference to the player's Transform

    private bool hasExploded = false;    // Ensures the fish only explodes once

    void Start()
    {
        // Find the player if not already assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (!hasExploded)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRadius)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        hasExploded = true;

        // Instantiate the explosion effect prefab
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Apply force to the player (if they have a Rigidbody)
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
        }

        // Deactivate the fish
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection and explosion radii in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
