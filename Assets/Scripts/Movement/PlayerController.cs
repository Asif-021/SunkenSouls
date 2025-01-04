using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using static Cinemachine.DocumentationSortingAttribute;

namespace SunkenSouls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float jumpForce;
        [SerializeField] float movementSpeed;
        [SerializeField] GameObject JellyFishGuide;

        private Rigidbody object_rigidBody;
        private Vector2 movementVector;
        private float jumpValue;

        public PlayableDirector deathCutsceneDirector;
        public PlayableDirector gameOverCutsceneDirector;
        public PlayableDirector levelFinishedCutsceneDirector;

        public static PlayerController instance;

        public static int lives;

        private static int playerHealth;

        private bool canTakeDamage;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (MainMenu.difficulty == DifficultyLevel.HARD)
            {
                JellyFishGuide.SetActive(false);
            }

            canTakeDamage = true;

            object_rigidBody = GetComponent<Rigidbody>();

            LivesLeftText.instance.SetText(lives);

            HealthBar.instance.ResetHealth();
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
                        
                        
                        StartCoroutine(LoadSceneAfterCutscene(levelFinishedCutsceneDirector, SceneManager.GetActiveScene().buildIndex + 1)); //!!!

                        // StartCoroutine(LoadSceneAfterCutscene(levelFinishedCutsceneDirector, SceneManager.GetActiveScene().buildIndex - 2));
                        // FOR TESTING^^^
                    }
                    break;
                case "Treasure":
                    FinalLevelManager.instance.DeleteFinalLevelManager();
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
                case "Battery":
                    DealDamage(-10);
                    Destroy(other.gameObject);
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
            if (transform.eulerAngles.x < 340 && transform.eulerAngles.x > 20)
            {
                movementDirection = new Vector3(0f, 0f, 0f);
            }

            object_rigidBody.AddRelativeForce(movementDirection * movementSpeed, ForceMode.Force);
        }


        void UpdateJump()
        {
            if (-0.1f < object_rigidBody.velocity.y && object_rigidBody.velocity.y < 0.1f)
            {
                object_rigidBody.AddForce(Vector3.up * jumpValue * jumpForce);
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
            if (canTakeDamage)
            {
                if (playerHealth == 0)
                {
                    if (lives > 0)
                    {
                        StartCoroutine(LoadSceneAfterCutscene(deathCutsceneDirector, SceneManager.GetActiveScene().buildIndex));

                        lives -= 1;
                        LivesLeftText.instance.SetText(lives);
                    }
                    else
                    {
                        StartCoroutine(LoadSceneAfterCutscene(gameOverCutsceneDirector, 0));
                    }

                    canTakeDamage = false;
                }
            }
        }

        private IEnumerator LoadSceneAfterCutscene(PlayableDirector director, int sceneIndex)
        {
            director.Play();
            yield return new WaitForSeconds((float)director.duration);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
