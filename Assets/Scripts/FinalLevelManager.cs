using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class FinalLevelManager : MonoBehaviour
    {
        private int healthDepletiontime = (MainMenu.difficulty == DifficultyLevel.EASY) ? 3 : 1;
        private int healthDepletionValue = (MainMenu.difficulty == DifficultyLevel.EASY) ? 10 : 20;

        private float previousTime;

        private void Start()
        {
            previousTime = Time.time;
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
    }
}
