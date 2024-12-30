using UnityEngine;

namespace SunkenSouls
{
    public class FriendlyFish : MonoBehaviour
    {
        public Transform playerTransform;  // Assign the Player or Camera Transform in the Inspector
        public Light fishLight;           // Reference to the Point Light component
        public float distanceFromPlayer = 2f;  // Distance in front of the player
        public float heightOffset = 1f;        // Height offset for the fish
        public string goldCoinTag = "GoldCoin_Collectible";  // Tag for gold coins
        public float coinDetectionRange = 20f; // Range within which the fish searches for coins

        private Transform nearestCoin;

        private void Start()
        {
            // Ensure the Point Light is enabled
            if (fishLight != null)
            {
                fishLight.enabled = true;
            }

            // Optionally, find the Player transform dynamically if not assigned
            if (playerTransform == null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        private void LateUpdate()
        {
            // Position the fish in front of the player and slightly to the right
            Vector3 forwardDirection = playerTransform.forward.normalized;

            // Add a small offset to the right (positive X direction)
            float rightOffset = 1f;  // You can adjust this value for the desired distance to the right
            Vector3 rightDirection = playerTransform.right.normalized;

            // Position the fish in front of the player, slightly to the right and with height offset
            transform.position = playerTransform.position + forwardDirection * distanceFromPlayer + Vector3.up * heightOffset + rightDirection * rightOffset;

            // Find the nearest gold coin
            FindNearestCoin();

            // Rotate towards the nearest coin if it exists, otherwise face the player
            if (nearestCoin != null)
            {
                transform.LookAt(nearestCoin);
            }
            else
            {
                transform.LookAt(playerTransform);
            }
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
    }
}
