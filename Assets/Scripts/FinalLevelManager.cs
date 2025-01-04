using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class FinalLevelManager : MonoBehaviour
    {
        private int healthDepletiontime;
        private int healthDepletionValue;

        private float previousTime;

        public static FinalLevelManager instance;

        private void Start()
        {
            healthDepletiontime = (MainMenu.difficulty == DifficultyLevel.EASY) ? 3 : 1;
            healthDepletionValue = (MainMenu.difficulty == DifficultyLevel.EASY) ? 10 : 20;

            instance = this;

            previousTime = Time.time;
            PlayerController.instance.DealDamage(healthDepletionValue);
        }

        private void Update()
        {
            float timeSpent = Time.time;
            if (timeSpent - previousTime >= healthDepletiontime) 
            {
                previousTime = timeSpent;
                PlayerController.instance.DealDamage(healthDepletionValue);                   
            }
        }

        public void DeleteFinalLevelManager()
        {
            Destroy(gameObject);
        }
    }
}
