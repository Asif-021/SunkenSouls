using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuControler : MonoBehaviour
{
    [SerializeField] private GameObject MenuRoot;
    [SerializeField] private GameObject HudRoot;

    private bool pauseMenuActivation = true;

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            SetPauseMenuActivation(pauseMenuActivation);
        }
    }
    public void SetPauseMenuActivation(bool active)
    {
        HudRoot.SetActive(!active);
        MenuRoot.SetActive(active);

        if (active)
        {   
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        pauseMenuActivation = !pauseMenuActivation;

    }

}
