using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SunkenSouls
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] GameObject HUD;
        [SerializeField] GameObject PauseMenu;
        [SerializeField] AudioClip hoverSound;

        private AudioSource object_audioSource;

        private void Start()
        {
            object_audioSource = GetComponent<AudioSource>();
        }

        public void MainMenu()
        {
            object_audioSource.Play();
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void Continue()
        {
            object_audioSource.Play();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;

            HUD.SetActive(true);
            PauseMenu.SetActive(false);
        }

        public void HoverSound()
        {
            object_audioSource.PlayOneShot(hoverSound);
        }
    }
}
