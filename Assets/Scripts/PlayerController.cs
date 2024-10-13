using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SunkenSouls
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // Player movement speed
        public float jumpForce = 8f; // Jump force
        public CharacterController controller; // Reference to the CharacterController

        private Vector3 moveDirection; // Movement direction
        private float verticalVelocity; // Vertical velocity (for jumping and gravity)
        private float gravity = -9.81f; // Gravity constant
        public Transform cameraTransform; // Reference to the camera's Transform


        //maximum number of possible pick ups
        private int numPickUps;

        // to store the number of pick ups collected
        private int pickupcount;

        // dislpay the collectibles collected text
        public TextMeshProUGUI collectedPickUps;
        public TextMeshProUGUI winText;

        void Start()
        {
            // setting the number of collectibles to be collected, which relies on the number of collectibles with a tag of 'PickUp'
            numPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;

            pickupcount = 0;
            controller = GetComponent<CharacterController>();

            // Assign the main camera's Transform if it's not already assigned
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "PickUp")
            {
                other.gameObject.SetActive(false);
                pickupcount++;
                SetPickUpsCollectedText();
            }
        }

        void Update()
        {
            // Get input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Get the forward and right directions relative to the camera
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Flatten the vectors to avoid vertical movement (y = 0)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Calculate the movement direction relative to the camera's orientation
            moveDirection = (forward * verticalInput + right * horizontalInput) * moveSpeed;

            // Handle jumping and gravity
            if (controller.isGrounded)
            {
                // Reset vertical velocity when grounded
                verticalVelocity = -1f;

                // Jump if the player presses the jump button
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = jumpForce;
                }
            }
            else
            {
                // Apply gravity when not grounded
                verticalVelocity += gravity * Time.deltaTime;
            }

            // Apply vertical velocity (gravity and jump) to the movement direction
            moveDirection.y = verticalVelocity;

            // Move the player
            controller.Move(moveDirection * Time.deltaTime);
        }

        private void SetPickUpsCollectedText()
        {
            collectedPickUps.text = "Collected Pick Ups: " + pickupcount.ToString();
            if (pickupcount >= numPickUps)
            {
                winText.text = "Congratulations! You completed this level!";
            }
        }
    }
}
