using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{


    public class ExplosiveFishController : MonoBehaviour
    {
        public enum FishState { Idle, Chasing, Warning, Exploding, Cooldown }
        private FishState currentState = FishState.Idle;

        public float detectionRadius = 10f; // Radius where the fish starts chasing the player
        public float chaseSpeed = 3f; // Speed of the fish while chasing
        public float warningDuration = 2f; // Time spent in the warning phase
        public float explosionRadius = 3f; // Explosion radius
        public float cooldownDuration = 5f; // Cooldown time after exploding
        public Transform player; // Reference to the player
        public GameObject explosionEffectPrefab; // Prefab for explosion effect

        private bool isExploded = false; // Prevents multiple explosions

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
            switch (currentState)
            {
                case FishState.Idle:
                    CheckPlayerProximity();
                    break;

                case FishState.Chasing:
                    ChasePlayer();
                    break;

                case FishState.Warning:
                    // Handled in coroutine
                    break;

                case FishState.Exploding:
                    // Handled in coroutine
                    break;

                case FishState.Cooldown:
                    // Do nothing here
                    break;
            }
        }

        private void CheckPlayerProximity()
        {
            // If the player enters the detection radius, start chasing
            if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                currentState = FishState.Chasing;
            }
        }

        private void ChasePlayer()
        {
            if (Vector3.Distance(transform.position, player.position) <= explosionRadius)
            {
                // Close enough to trigger the warning phase
                StartCoroutine(WarningPhase());
            }
            else
            {
                // Chase the player
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * chaseSpeed * Time.deltaTime;
            }
        }

        private IEnumerator WarningPhase()
        {
            currentState = FishState.Warning;

            // Example warning effects: scale up and flash red
            float elapsed = 0f;
            Vector3 originalScale = transform.localScale;
            Vector3 warningScale = originalScale * 1.5f;

            while (elapsed < warningDuration)
            {
                transform.localScale = Vector3.Lerp(originalScale, warningScale, elapsed / warningDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = warningScale;

            // Trigger explosion
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            currentState = FishState.Exploding;

            // Spawn explosion effect
            if (explosionEffectPrefab)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // Apply explosion force to the player
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 explosionDirection = (player.position - transform.position).normalized;
                playerRb.AddForce(explosionDirection * -10f, ForceMode.Impulse);
            }

            // Disable fish temporarily
            gameObject.SetActive(false);

            yield return new WaitForSeconds(cooldownDuration);

            // Reset fish
            isExploded = false;
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            currentState = FishState.Idle;
        }

        void OnDrawGizmosSelected()
        {
            // Visualize detection and explosion radii in the editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }


}
