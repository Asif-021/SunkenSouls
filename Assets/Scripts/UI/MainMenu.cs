using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SunkenSouls
{
    public enum DifficultyLevel{
        EASY,
        HARD
    }

    public class MainMenu : MonoBehaviour
    {
        [SerializeField] Slider difficultySlider;

        public static DifficultyLevel difficulty;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void StartGame()
        {
            SetPlayerLives();
            HealthBar.currentHealth = 100;
            int level = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(level + 1);
        }

        private void SetPlayerLives()
        {
            if (difficultySlider.value == 0)
            {
                difficulty = DifficultyLevel.EASY;
                PlayerController.lives = 3;
            } 
            else
            {
                difficulty = DifficultyLevel.HARD;
                PlayerController.lives = 1;
            }
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
