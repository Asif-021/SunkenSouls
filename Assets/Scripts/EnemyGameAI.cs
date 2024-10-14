using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Required for using UI components like Image

namespace SunkenSouls
{
    public class EnemyGameAI : MonoBehaviour
    {
        public Transform player;
        public List<Transform> wayPoint;  // List of waypoints for patrolling
        private NavMeshAgent agent;

        // Patrolling variables
        private int currentWayPointIndex = 0;

        // Chase and attack variables
        private float chaseRangeAndAttackRange = 5f;
        private bool isChasing = false;
        private bool isFlashing = false;

        // UI for attack indication
        public Image attackImage;
        private float flashDuration = 0.5f;
        private Coroutine flashingCoroutine;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // Ensure the attack image is initially invisible
            if (attackImage != null)
            {
                attackImage.enabled = false;
            }
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (isChasing)
            {
                agent.destination = player.position;

                if (distanceToPlayer > chaseRangeAndAttackRange)
                {
                    // Stop flashing the image if the player is out of range
                    if (isFlashing)
                    {
                        StopCoroutine(flashingCoroutine);
                        flashingCoroutine = null;
                        attackImage.enabled = false; // Ensure the image is hidden
                        isFlashing = false;
                    } 

                }
                else
                {
                    if (!isFlashing)
                    {
                        flashingCoroutine = StartCoroutine(FlashImage());
                    }
                }
            }

            else
            {
                // If the player is in chase/attack range, start chasing and stop patrolling
                if (distanceToPlayer <= chaseRangeAndAttackRange)
                {
                    if (!isChasing)
                    {
                        isChasing = true;
                        StopPatrolling();
                        agent.destination = player.position;
                    }

                    // Flash the attack image while chasing/attacking the player
                    if (!isFlashing)
                    {
                        flashingCoroutine = StartCoroutine(FlashImage());

                    }
                }
            }

            // Patrol if not chasing the player
            if (!isChasing)
            {
                Patrol();
            }
        }

        // Patrol between waypoints
        private void Patrol()
        {
            if (wayPoint.Count == 0) return;

            float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, transform.position);
            if (distanceToWayPoint <= 1)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;
            }
            agent.SetDestination(wayPoint[currentWayPointIndex].position);
        }

        // Stop patrolling when chasing the player
        private void StopPatrolling()
        {
            agent.ResetPath();
        }

        // Coroutine to continuously flash the attack image while the player is in range
        private IEnumerator FlashImage()
        {
            isFlashing = true; 

            while (true) 
            {
                // Show the image
                attackImage.enabled = true;

                // Wait for the flash duration
                yield return new WaitForSeconds(flashDuration);

                // Hide the image
                attackImage.enabled = false;

                // Wait for the same duration before flashing again
                yield return new WaitForSeconds(flashDuration);
            }
        }
    }
}
