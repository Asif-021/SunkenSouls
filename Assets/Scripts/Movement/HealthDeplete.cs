using System.Collections;
using UnityEngine;

namespace SunkenSouls
{
    public class HealthDeplete : MonoBehaviour
    {
        public float maxHealth = 100f;      // Maximum health
        public float currentHealth;        // Current health level
        public float depletionRate = 1f;   // Health depletion per second
        public int damagePerTick = 1;      // Damage dealt per second

        private void Start()
        {
            // Initialize health to maximum level
            currentHealth = maxHealth;
            StartCoroutine(DepleteHealthOverTime());
        }

        private IEnumerator DepleteHealthOverTime()
        {
            while (currentHealth > 0)
            {
                // Deal damage to the player over time
                PlayerController.instance.DealDamage(damagePerTick);

                // Update current health for internal tracking
                currentHealth -= depletionRate;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

                yield return new WaitForSeconds(1f); // Damage every second
            }

            // If health reaches zero, logic could be expanded here
        }

        public void ReplenishHealth(float amount)
        {
            // Add to health and clamp to the max value
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
}
