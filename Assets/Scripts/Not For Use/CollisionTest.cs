using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunkenSouls
{
    public class CollisionTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "enemy")
            {
                print("ENTER");
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "enemy")
            {
                print("STAY");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "enemy")
            {
                print("EXIT");
            }
        }
    }
}
