using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

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

        public static int lives;

        private static int playerHealth;

        void Start()
        {
            object_rigidBody = GetComponent<Rigidbody>();

            LivesLeftText.instance.SetText(lives);
            playerHealth = HealthBar.instance.GetHealth();

            CoinsCollectedText.instance.SetCoinsRequired(GameObject.FindGameObjectsWithTag("GoldCoin_Collectible").Length);

            instance = this;
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
                case "NextLevelDoor":
                    if (CoinsCollectedText.instance.GetCoinsToCollect() == 0)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
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

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "GoldCoin_Collectible":
                    CoinsCollectedText.instance.UpdateCoinsCollected();
                    other.gameObject.SetActive(false);
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
            UpdateJump();
            UpdatePosition();
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
            playerHealth = HealthBar.instance.GetHealth();

            UpdateHealthData();
        }

        private void UpdateHealthData()
        {
            if (playerHealth == 0)
            {
                if (lives == 0)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    object_rigidBody.constraints = RigidbodyConstraints.FreezeAll;

                    lives -= 1;                
                    LivesLeftText.instance.SetText(lives);
                    HealthBar.instance.ResetHealth();
                }
            }
        }
    }
}
