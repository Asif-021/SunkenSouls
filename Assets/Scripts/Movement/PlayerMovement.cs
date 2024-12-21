using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SunkenSouls
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        [SerializeField] float movementSpeed;
        [SerializeField] Camera playerCamera;

        private Rigidbody object_rigidBody;
        private Vector2 movementVector;
        private float jumpValue;

        void Start()
        {
            object_rigidBody = GetComponent<Rigidbody>();
        }

        void OnMove(InputValue movementValue)
        {
            movementVector = movementValue.Get<Vector2>();
        }

        void OnJump(InputValue value)
        {
            jumpValue = value.Get<float>();
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateJump();
        }

        void UpdatePosition()
        {
            Vector3 forward = playerCamera.transform.forward;
            Vector3 right = playerCamera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * movementVector.y + right * movementVector.x).normalized;
            object_rigidBody.AddForce(moveDirection * movementSpeed * Time.fixedDeltaTime, ForceMode.Force);
        }

        void UpdateJump()
        {
            object_rigidBody.AddForce(Vector3.up * jumpValue * jumpForce * Time.fixedDeltaTime);
            jumpValue = 0.0f;
        }
    }
}