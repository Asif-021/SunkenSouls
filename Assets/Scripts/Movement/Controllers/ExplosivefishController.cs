using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class ExplosiveFishController : MonoBehaviour
    {
        // Enumeration to define the states of the fish
        public enum FishState { Idle, Chasing, Warning, Exploding, Cooldown }
        private FishState currentState = FishState.Idle;

        // Public variables for tuning fish behavior
        public float detectionRange = 10f;         // How far the fish can detect the player
        public float detectionAngle = 60f;        // Detection cone angle (30° on each side)
        public float chaseSpeed = 3f;             // Speed of the fish while chasing
        public float rotationSpeed = 5f;         // Speed of rotation toward the player
        public float warningDuration = 2f;       // Duration of the warning phase
        public float explosionRadius = 3f;       // Radius of the explosion
        public float explosionTriggerRange = 2f; // Distance required to trigger the explosion
        public float cooldownDuration = 5f;      // Cooldown time after exploding
        public float lostPlayerDuration = 3f;    // Time to wait before resuming patrol after losing the player
        public Transform player;                 // Reference to the player
        public GameObject explosionEffectPrefab; // Prefab for the explosion effect

        private bool isExploded = false;         // Flag to prevent multiple explosions
        private float timeSincePlayerSeen = 0f;  // Timer for losing the player

        // Reference to the PatrolController
        private PatrolController patrolController;

        void Start()
        {
            // If player is not manually assigned, find it using the "Player" tag
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            // Get the PatrolController component
            patrolController = GetComponent<PatrolController>();
        }

        void Update()
        {
            // Handle behavior based on the current state
            switch (currentState)
            {
                case FishState.Idle:
                    CheckPlayerVisibility(); // Check if the player is detected
                    break;

                case FishState.Chasing:
                    ChasePlayer(); // Move toward the player
                    break;

                case FishState.Warning:
                case FishState.Exploding:
                case FishState.Cooldown:
                    // These states are handled in coroutines
                    break;
            }
        }

        private void CheckPlayerVisibility()
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Get the distance and angle between the fish and the player
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // If within detection range and cone, switch to Chasing state
            if (distanceToPlayer <= detectionRange && angleToPlayer <= detectionAngle / 2f)
            {
                currentState = FishState.Chasing;

                // Stop patrolling when switching to chasing
                if (patrolController != null)
                {
                    patrolController.StopPatrolling();
                }
            }
        }

        private void ChasePlayer()
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // If the player is within explosion trigger range, start the Warning phase
            if (distanceToPlayer <= explosionTriggerRange)
            {
                StartCoroutine(WarningPhase());
                return;
            }

            // Rotate toward the player for more accurate chasing
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the fish toward the player
            transform.position += direction * chaseSpeed * Time.deltaTime;

            // Handle losing the player
            HandleLostPlayer(directionToPlayer: direction, distanceToPlayer: distanceToPlayer);
        }

        private void HandleLostPlayer(Vector3 directionToPlayer, float distanceToPlayer)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (distanceToPlayer > detectionRange || angleToPlayer > detectionAngle / 2f)
            {
                timeSincePlayerSeen += Time.deltaTime;

                if (timeSincePlayerSeen >= lostPlayerDuration)
                {
                    ResumePatrolling();
                }
            }
            else
            {
                timeSincePlayerSeen = 0f; // Reset timer if the player is seen again
            }
        }

        private void ResumePatrolling()
        {
            currentState = FishState.Idle;

            // Resume patrolling when player is lost
            if (patrolController != null)
            {
                patrolController.ResumePatrolling();
            }
        }

        private IEnumerator WarningPhase()
        {
            // Enter the Warning state
            currentState = FishState.Warning;

            // Visual warning effect: Scale up over time
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

            // Trigger the explosion
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            // Enter the Exploding state
            currentState = FishState.Exploding;

            // Spawn the explosion effect
            if (explosionEffectPrefab)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // Apply force to the player if they have a Rigidbody
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 explosionDirection = (player.position - transform.position).normalized;
                playerRb.AddForce(explosionDirection * -10f, ForceMode.Impulse);
            }

            // Disable the fish temporarily
            gameObject.SetActive(false);

            // Wait for the cooldown duration before resetting
            yield return new WaitForSeconds(cooldownDuration);

            isExploded = false;
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
            ResumePatrolling();
        }

        void OnDrawGizmosSelected()
        {
            // Visualize detection range and explosion range in the editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange); // Detection range

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, explosionTriggerRange); // Explosion trigger range

            Gizmos.color = Color.red;
            Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward;

            // Draw the detection cone
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);
        }
    }
}
