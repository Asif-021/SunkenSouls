using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class FirstPersonCamera : MonoBehaviour
    {
        public Transform player; // Reference to the player
        public float mouseSensitivity = 2f; // Mouse sensitivity
        private float cameraVerticalRotation = 0f; // Vertical rotation of the camera

        void Start()
        {
            // Lock and hide the cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            // Get mouse input
            float inputX = Input.GetAxis("Mouse X") * mouseSensitivity; // Horizontal mouse movement
            float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity; // Vertical mouse movement

            // Rotate the camera vertically (up/down) and clamp it between -90 and 90 degrees
            cameraVerticalRotation -= inputY;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
            transform.localEulerAngles = new Vector3(cameraVerticalRotation, 0f, 0f);

            // Rotate the player horizontally (left/right)
            player.Rotate(Vector3.up * inputX);
        }
    }
}
