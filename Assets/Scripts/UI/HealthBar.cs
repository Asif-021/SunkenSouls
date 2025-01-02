using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SunkenSouls
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image foreground;

        public static int currentHealth = 100;
        private int maxHealth = 100;

        public static HealthBar instance;

        void Awake()
        {
            instance = this;
            SetHealth();
        }

        public void DecreaseHealth(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            SetHealth();
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public void ResetHealth()
        {
            currentHealth = 100;
            SetHealth();
        }

        private void SetHealth()
        {
            foreground.fillAmount = (float) currentHealth / maxHealth;
        }
    }
}
