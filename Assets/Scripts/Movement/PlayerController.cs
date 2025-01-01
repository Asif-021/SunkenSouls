using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SunkenSouls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        [SerializeField] float movementSpeed;

        private Rigidbody object_rigidBody;
        private Vector2 movementVector;
        private float jumpValue;

        public static PlayerController instance;

        void Start()
        {
            instance = this;
            object_rigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "MovingPlatform":
                    transform.SetParent(collision.gameObject.transform.parent, true);
                    break;
                case "Spears":
                    DealDamage(100);
                    break;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "MovingPlatform":
                    transform.SetParent(null, true);
                    break;
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

        public void DealDamage(int damageAmount)
        {
            HealthBar.instance.DecreaseHealth(damageAmount);
        }
    }
}
