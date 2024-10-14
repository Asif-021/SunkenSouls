using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SunkenSouls
{
    public class PatrollingEnemyAI : MonoBehaviour
    {

        public List<Transform> patrolPoint;
        NavMeshAgent navMeshAgent;

        public int currentPatrolPoint = 0;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

        }

        private void Update()
        {
            Patrolling();
        }

        void Patrolling()
        {
            //if no patrol points are set, return
            if (patrolPoint.Count == 0)
            {
                return;
            }

            float distanceToNextPoint = Vector3.Distance(patrolPoint[currentPatrolPoint].position, transform.position);

            //if the enemy is close to the next patrol point switch the patrol point
            if (distanceToNextPoint <= 1)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoint.Count;
            }

            //After patrol point has changed, move the enemy ai towards the 'new' patrol point
            navMeshAgent.SetDestination(patrolPoint[currentPatrolPoint].position);
        }
    }
}

