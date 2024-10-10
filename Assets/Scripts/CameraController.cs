using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;

        public bool useOffsetValues;

        public float rotateSpeed;

        public Transform pivot;

        // Minimum height above the player
        public float minHeightAboveTarget = 1.5f;

        // Start is called before the first frame update
        void Start()
        {
            if (!useOffsetValues)
            {
                offset = target.position - transform.position;
            }

            pivot.transform.position = target.transform.position;
            pivot.transform.parent = target.transform;

            Cursor.lockState = CursorLockMode.Locked; // Locks mouse into the game, press the esc button for mouse to reappear
        }

        // Update is called once per frame
        void Update()
        {
            // Get the x position of the mouse and rotate the target
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            target.Rotate(0, horizontal, 0);

            // Get the Y position of the mouse and rotate the pivot 
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            pivot.Rotate(-vertical, 0, 0);

            // Move the camera based on the current rotation of the target and the original offset
            float desiredYAngle = target.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            Vector3 newPosition = target.position - (rotation * offset);

            // Adjust the camera's height to ensure it doesn't go below the target
            if (newPosition.y < target.position.y + minHeightAboveTarget)
            {
                newPosition.y = target.position.y + minHeightAboveTarget; // Set to minimum height above the player
            }

            transform.position = newPosition;

            transform.LookAt(target);
        }
    }
}
