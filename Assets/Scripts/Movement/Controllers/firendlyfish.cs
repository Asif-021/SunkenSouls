using UnityEngine;

namespace SunkenSouls
{
    public class FriendlyFish : MonoBehaviour
    {
        public enum FishState { Idle, Guiding, Warning }
        private FishState currentState = FishState.Idle;

        public Transform playerTransform;    // Assign the Player or Camera Transform in the Inspector
        public Light fishLight;             // Reference to the Point Light component
        public float distanceFromPlayer = 2f; // Distance in front of the player
        public float heightOffset = 1f;       // Height offset for the fish
        public string goldCoinTag = "GoldCoin_Collectible"; // Tag for gold coins
        public string enemyTag = "Enemy";    // Tag for enemies
        public float coinDetectionRange = 20f; // Range within which the fish searches for coins
        public float warningRange = 10f;     // Range within which the fish warns about enemies

        private Transform nearestCoin;
        private Transform nearestEnemy;

        private void Start()
        {
            // Ensure the Point Light is enabled
            if (fishLight != null)
            {
                fishLight.enabled = true;
                fishLight.color = Color.white; // Default light colour
            }

            // Dynamically find the Player transform if not assigned
            if (playerTransform == null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        private void Update()
        {
            switch (currentState)
            {
                case FishState.Idle:
                    IdleBehaviour();
                    break;

                case FishState.Guiding:
                    GuidingBehaviour();
                    break;

                case FishState.Warning:
                    WarningBehaviour();
                    break;
            }
        }

        private void IdleBehaviour()
        {
            FollowPlayer();

            // Check for the nearest coin
            FindNearestCoin();
            if (nearestCoin != null)
            {
                currentState = FishState.Guiding;
            }

            // Check for nearby enemies
            FindNearestEnemy();
            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.position) <= warningRange)
            {
                currentState = FishState.Warning;
            }
        }

        private void GuidingBehaviour()
        {
            FollowPlayer();

            // Smoothly rotate to face the nearest coin
            if (nearestCoin != null)
            {
                Vector3 directionToCoin = (nearestCoin.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(directionToCoin);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust 5f for rotation speed
            }

            // If no coins are nearby, return to Idle state
            FindNearestCoin();
            if (nearestCoin == null)
            {
                currentState = FishState.Idle;
            }

            // Check for nearby enemies
            FindNearestEnemy();
            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.position) <= warningRange)
            {
                currentState = FishState.Warning;
            }
        }


        private void WarningBehaviour()
        {
            FollowPlayer(); // Continue following the player

            fishLight.color = Color.red; // Change light to red as a warning
            Debug.Log("Warning: Enemy nearby!");

            // Check if the enemy is still in range
            FindNearestEnemy();
            if (nearestEnemy == null || Vector3.Distance(transform.position, nearestEnemy.position) > warningRange)
            {
                fishLight.color = Color.white; // Reset light to default
                currentState = FishState.Idle;
            }
        }


        private void FollowPlayer()
        {
            Vector3 forwardDirection = playerTransform.forward.normalized;
            Vector3 rightDirection = playerTransform.right.normalized;
            transform.position = playerTransform.position + forwardDirection * distanceFromPlayer + Vector3.up * heightOffset + rightDirection;
        }

        private void FindNearestCoin()
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag(goldCoinTag);
            float shortestDistance = Mathf.Infinity;
            nearestCoin = null;

            foreach (GameObject coin in coins)
            {
                float distanceToCoin = Vector3.Distance(transform.position, coin.transform.position);
                if (distanceToCoin < shortestDistance && distanceToCoin <= coinDetectionRange)
                {
                    shortestDistance = distanceToCoin;
                    nearestCoin = coin.transform;
                }
            }
        }

        private void FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistance = Mathf.Infinity;
            nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance && distanceToEnemy <= warningRange)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, coinDetectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, warningRange);
        }
    }
}
