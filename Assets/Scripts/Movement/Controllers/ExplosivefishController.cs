using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Transform player;
        public GameObject explosionEffectPrefab;

        private PatrolController patrolController;
        private AudioSource audioSource;

        void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            patrolController = GetComponent<PatrolController>();

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing!");
            }

            audioSource.playOnAwake = false;
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
        }

        private IEnumerator WarningPhase()
        {
            currentState = FishState.Warning;

            float elapsed = 0f;
            Vector3 originalScale = transform.localScale;
            Vector3 warningScale = originalScale * 1.5f;

            // Perform the "charge-up" scaling animation
            while (elapsed < warningDuration)
            {
                transform.localScale = Vector3.Lerp(originalScale, warningScale, elapsed / warningDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = warningScale; // Ensure final scale is set
            TriggerExplosion();
        }

        private void TriggerExplosion()
        {
            currentState = FishState.Exploding;

            // Spawn the explosion effect
            if (explosionEffectPrefab)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // Play the explosion sound
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }

            // Check if the player is within the explosion radius
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= explosionRadius)
            {
                int explosionDamage = MainMenu.difficulty == DifficultyLevel.HARD ? 65 : 40;
                PlayerController.instance.DealDamage(explosionDamage);
            }

            // Destroy all child objects but keep the parent
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Optionally, disable the collider if present
            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Destroy the parent after the sound has finished playing (optional)
            Destroy(gameObject, audioSource.clip.length);
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
