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
        [SerializeField] AudioClip hoverSound;

        [SerializeField] GameObject EasyModeDescriptor;
        [SerializeField] GameObject HardModeDescriptor;

        [SerializeField] Slider DifficultySelector;

        public static DifficultyLevel difficulty;
        private AudioSource object_audioSource;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            object_audioSource = GetComponent<AudioSource>();
        }

        public void StartGame()
        {
            object_audioSource.Play();

            SetPlayerLives();
            HealthBar.currentHealth = 100;
            int level = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(level + 1);

            // SceneManager.LoadScene(level + 4);
            // FOR TESTING^^^
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
            object_audioSource.Play();
            Application.Quit();
        }

        public void HoverSound()
        {
            object_audioSource.PlayOneShot(hoverSound);
        }

        public void EasyModeSelected()
        {
            HoverSound();
            EasyModeDescriptor.SetActive(true);
        }

        public void HardModeSelected()
        {
            HoverSound();
            HardModeDescriptor.SetActive(true);
        }

        public void ExitModeButtons()
        {
            if (EasyModeDescriptor.activeSelf)
            {
                EasyModeDescriptor.SetActive(false);
            }

            if (HardModeDescriptor.activeSelf)
            {
                HardModeDescriptor.SetActive(false);
            }
        }

        public void SetHardMode()
        {
            object_audioSource.Play();
            DifficultySelector.value = 1;
        }

        public void SetEasyMode()
        {
            object_audioSource.Play();
            DifficultySelector.value = 0;
        }
    }
}
