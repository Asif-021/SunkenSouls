using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; // Required for using UI components like Image

namespace SunkenSouls
{
    public class EnemyGameAI : MonoBehaviour
    {
        public Transform player;
        private NavMeshAgent agent;

        private float chaseRange = 3f;
        private bool chaseStarted = false;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (chaseStarted)
            {
                agent.destination = player.position;
            }
            else
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // If the player is within the chase range, set the destination to the player's position
                if (distanceToPlayer <= chaseRange)
                {
                    chaseStarted = true;
                    agent.destination = player.position;
                }
            }
        }
    }
}