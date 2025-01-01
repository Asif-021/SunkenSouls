using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;  // Required for Timeline

public class CutsceneTrigger : MonoBehaviour
{


    public PlayableDirector cutsceneDirector;  // Reference to the PlayableDirector (Timeline)
    private void OnTriggerEnter(Collider other)
    {
       
        cutsceneDirector.Play();

    }
}