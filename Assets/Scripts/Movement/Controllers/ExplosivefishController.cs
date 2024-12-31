using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SunkenSouls
{
    public class ExplosiveFishController : MonoBehaviour
    {
        public enum FishState { Idle, Chasing, Warning, Exploding, Cooldown }
        private FishState currentState = FishState.Idle;

        public float detectionRange = 10f;
        public float detectionAngle = 60f;
        public float chaseSpeed = 3f;
        public float rotationSpeed = 5f;
        public float warningDuration = 2f;
        public float explosionRadius = 3f;
        public float explosionTriggerRange = 2f;
        public float cooldownDuration = 5f;
        public float lostPlayerDuration = 3f;
        public Transform player;
        public GameObject explosionEffectPrefab;

        public Transform respawnPoint; // Reference to the respawn point

        private bool isExploded = false;
        private float timeSincePlayerSeen = 0f;
        private PatrolController patrolController;

        void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            patrolController = GetComponent<PatrolController>();
        }

        void Update()
        {
            switch (currentState)
            {
                case FishState.Idle:
                    CheckPlayerVisibility();
                    break;

                case FishState.Chasing:
                    ChasePlayer();
                    break;

                case FishState.Warning:
                case FishState.Exploding:
                case FishState.Cooldown:
                    break;
            }
        }

        private void CheckPlayerVisibility()
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (distanceToPlayer <= detectionRange && angleToPlayer <= detectionAngle / 2f)
            {
                currentState = FishState.Chasing;

                if (patrolController != null)
                {
                    patrolController.StopPatrolling();
                }
            }
        }

        private void ChasePlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= explosionTriggerRange)
            {
                StartCoroutine(WarningPhase());
                return;
            }

            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.position += direction * chaseSpeed * Time.deltaTime;

            HandleLostPlayer(direction, distanceToPlayer);
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
                timeSincePlayerSeen = 0f;
            }
        }

        private void ResumePatrolling()
        {
            currentState = FishState.Idle;

            if (patrolController != null)
            {
                patrolController.ResumePatrolling();
            }
        }

        private IEnumerator WarningPhase()
        {
            currentState = FishState.Warning;

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
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            currentState = FishState.Exploding;

            // Spawn the explosion effect
            if (explosionEffectPrefab)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // Check if the player is within the explosion radius
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= explosionRadius)
            {
                // Reload the current scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                yield break; // Stop further execution since the scene is reloading
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, explosionTriggerRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);

            Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward;
            Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward;

            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);
        }
    }
}
