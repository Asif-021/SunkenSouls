using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

namespace SunkenSouls
{
    public class CutsceneManager : MonoBehaviour
    {
        public PlayableDirector EasyModeCutsceneDirector;
        public PlayableDirector HardModeCutsceneDirector;

        private void Start()
        {
            StartCoroutine(PlayDifficultyCutscene());
        }

        private IEnumerator PlayDifficultyCutscene()
        {
            if (MainMenu.difficulty == DifficultyLevel.HARD)
            {
                HardModeCutsceneDirector.Play();
                yield return new WaitForSeconds((float)HardModeCutsceneDirector.duration);
            }
            else
            {
                EasyModeCutsceneDirector.Play();
                yield return new WaitForSeconds((float)EasyModeCutsceneDirector.duration);
            }

            LoadLevel1Scene();
        }

        private void LoadLevel1Scene()
        {
            // Load the actual Level 1 scene (or any subsequent scene)
            SceneManager.LoadScene("Level 1");
        }
    }
}
