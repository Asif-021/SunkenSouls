using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;  // Required for Timeline

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector cutsceneDirector;  
    public int nextSceneIndex;  

    private void OnTriggerEnter(Collider other)
    {
       
        cutsceneDirector.Play();  // Play the cutscene
        StartCoroutine(LoadNextSceneAfterCutscene());
        
    }

    private IEnumerator LoadNextSceneAfterCutscene()
    {
        // Wait for the cutscene to complete
        yield return new WaitForSeconds((float)cutsceneDirector.duration);

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }
}