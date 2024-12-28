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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "MovingPlatform")
            {
                transform.SetParent(collision.gameObject.transform, true);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "MovingPlatform")
            {
                transform.SetParent(null, true);
            }
        }

        void OnMove(InputValue movementValue)
        {
            movementVector = movementValue.Get<Vector2>();
        }

        void OnJump(InputValue value)
        {
            jumpValue = value.Get<float>();
        }

        void Update()
        {
            // temporarily accessing the main camera until Unity support gets back (as ssignign the camera to a serializeable field does not give its rotation)
            transform.rotation = Camera.main.transform.rotation;
        }

        private void FixedUpdate()
        {
            UpdatePosition();
            UpdateJump();
        }

        void UpdatePosition()
        {
            Vector3 movementDirection = new Vector3(movementVector.x, 0.0f, movementVector.y);
            object_rigidBody.AddRelativeForce(movementDirection * movementSpeed * Time.fixedDeltaTime, ForceMode.Force);
        }


        void UpdateJump()
        {
            if (-0.1f < object_rigidBody.velocity.y  && object_rigidBody.velocity.y < 0.1f)
            {
                object_rigidBody.AddForce(Vector3.up * jumpValue * jumpForce * Time.fixedDeltaTime);
                jumpValue = 0.0f;
            }
        }
    }
}
