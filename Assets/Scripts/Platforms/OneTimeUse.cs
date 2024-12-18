using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class OneTimeUse : MonoBehaviour
    {
        [SerializeField] float time_to_destroy_after_collision;

        private float timeOf_collision;
        private bool collided;

        private void Update()
        {
            if (collided)
            {
                if (Time.time - timeOf_collision >= time_to_destroy_after_collision)
                {
                    Destroy(gameObject);
                }
            }
        }

        void OnCollisionEnter(Collision other)
        {   
            // when the player collides with the platform
            // the collision time is stored and the object turns red, to indicate incoming destruction
            if (!collided)
            {
                collided = true;
                timeOf_collision = Time.time;

                GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
