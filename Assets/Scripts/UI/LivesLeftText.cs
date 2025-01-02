using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SunkenSouls
{
    public class LivesLeftText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI livesLeft;

        public static LivesLeftText instance;

        private void Awake()
        {
            instance = this;
        }

        public void SetText(int lives)
        {
            livesLeft.text = "You have " + lives + " lives left";
        }
    }
}
