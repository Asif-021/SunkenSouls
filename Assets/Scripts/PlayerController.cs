using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed;
        public float jumpForce;
        public CharacterController controller;
        public Camera playerCamera; // Reference to the player's camera

        private Vector3 moveDirection;
        private float verticalVelocity; // Separate vertical velocity
        private float gravity = -9.81f; // Earth's gravity

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<CharacterController>();
            // Ensure to assign the camera if it's not assigned in the inspector
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Get input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Get camera forward and right vectors
            Vector3 forward = playerCamera.transform.forward;
            Vector3 right = playerCamera.transform.right;

            // Flatten the vectors to ignore the y-axis
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            // Calculate the move direction relative to the camera
            moveDirection = (right * horizontalInput + forward * verticalInput) * moveSpeed;

            // Check if the player is on the ground
            if (controller.isGrounded)
            {
                // If grounded, reset the vertical velocity
                verticalVelocity = -1f; // Slightly negative to ensure the player stays grounded

                // Handle jumping
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = jumpForce;
                }
            }
            else
            {
                // Apply gravity over time when not grounded
                verticalVelocity += gravity * Time.deltaTime;
            }

            // Add the vertical velocity to moveDirection
            moveDirection.y = verticalVelocity;

            // Move the controller
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
