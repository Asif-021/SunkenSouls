using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SunkenSouls
{
    public class EnemyAI : MonoBehaviour
    {

        public List<Transform> wayPoint;
        NavMeshAgent navMeshAgent;

        public int currentWayPointIndex = 0;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();

        }

        private void Update()
        {
            Walking();
        }

        void Walking()
        {
            if(wayPoint.Count == 0)
            {
                return;
            }

            float distanceToWayPoint = Vector3.Distance(wayPoint[currentWayPointIndex].position, transform.position);

            if(distanceToWayPoint <= 1)
            {
                currentWayPointIndex = (currentWayPointIndex + 1) % wayPoint.Count;
            }

            navMeshAgent.SetDestination(wayPoint[currentWayPointIndex].position);
        }
    }
}
