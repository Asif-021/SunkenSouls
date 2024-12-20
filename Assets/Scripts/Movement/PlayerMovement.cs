using UnityEngine;
using UnityEngine.InputSystem;

namespace SunkenSouls
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        [SerializeField] float movementSpeed;

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
            Vector3 newPosition = new Vector3(movementVector.x, 0.0f, movementVector.y);
            object_rigidBody.AddForce(newPosition * movementSpeed * Time.fixedDeltaTime);
        }

        void UpdateJump()
        {
            object_rigidBody.AddForce(Vector3.up * jumpValue * jumpForce * Time.fixedDeltaTime);
            jumpValue = 0.0f;
        }
    }
}
