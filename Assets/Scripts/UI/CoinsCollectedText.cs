using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SunkenSouls
{
    public class CoinsCollectedText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI coinsCollectedText;

        private int coinsToCollect;

        public static CoinsCollectedText instance;

        private void Awake()
        {
            instance = this;
        }

        public void SetCoinsRequired(int requiredCoins)
        {
            coinsToCollect = requiredCoins;
            SetText();
        }

        public void UpdateCoinsCollected()
        {
            coinsToCollect -= 1;
            SetText();
        }

        public int GetCoinsToCollect()
        {
            return coinsToCollect;
        }

        private void SetText()
        {
            coinsCollectedText.text = "You need to collect " + coinsToCollect + " coins";
        }
    }
}
