using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SunkenSouls
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField] float jumpPad_force;

        void OnCollisionEnter(Collision other) {
            Rigidbody foreignObject_rigidBody = other.gameObject.GetComponent<Rigidbody>();
            foreignObject_rigidBody.AddForce(Vector3.up * jumpPad_force * Time.deltaTime);
        }
    }
}
