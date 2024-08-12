using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float projectileSpeed;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        rb.velocity = transform.forward * projectileSpeed;
    }
}
