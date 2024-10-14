using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Required for using UI components like Image
using System.Collections; // Required for Coroutine

namespace SunkenSouls
{
    public class EnemyGameAI : MonoBehaviour
    {
        public Transform player;
        private NavMeshAgent agent;

        public float chaseRangeAndAttackRange = 5f;
        private bool chaseStarted = false;
        private bool isFlashing = false;

        public Image attackImage;
        private float flashDuration = 0.5f;

        private Coroutine flashingCoroutine;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            // Ensure the image is initially invisible
            attackImage.enabled = false;
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Chase starts if it's not already started
            if (chaseStarted)
            {
                agent.destination = player.position;
            }
            else
            {
                if (distanceToPlayer <= chaseRangeAndAttackRange)
                {
                    chaseStarted = true;
                    agent.destination = player.position;
                }
            }

            // If the player is within the chase and attack range, start flashing the image
            if (distanceToPlayer <= chaseRangeAndAttackRange)
            {
                if (!isFlashing)
                {
                    flashingCoroutine = StartCoroutine(FlashImage());
                }
            }
            else
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
        }

        // Coroutine to continuously flash the attack image while the player is in range
        private IEnumerator FlashImage()
        {
            isFlashing = true; // Prevent the coroutine from being called multiple times

            while (true) // Loop indefinitely while the coroutine is running
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
