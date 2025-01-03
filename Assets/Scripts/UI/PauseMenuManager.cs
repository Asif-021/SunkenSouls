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

        public void MainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void Continue()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;

            HUD.SetActive(true);
            PauseMenu.SetActive(false);
        }
    }
}
