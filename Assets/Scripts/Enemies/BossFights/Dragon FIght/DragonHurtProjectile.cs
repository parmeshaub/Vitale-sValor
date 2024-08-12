using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHurtProjectile : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform target; // Reference to the target (e.g., the player)
    private GameObject dragonboss;
    private DragonBoss dragon;
    [SerializeField] private GameObject explosionPrefab; // Explosion prefab to instantiate on collision

    private void Start() {
        rb = GetComponent<Rigidbody>();
        dragonboss = GameObject.FindGameObjectWithTag("Enemy");
        dragon = dragonboss.GetComponent<DragonBoss>();
        target = dragonboss.transform;

    }

    private void FixedUpdate() {
        // Move the projectile forward
        rb.velocity = transform.forward * speed;

        // Rotate the projectile towards the target
        RotateProjectile();
    }

    private void RotateProjectile() {
        if (target != null) {
            // Calculate the direction to the target
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Calculate the rotation step based on the rotation speed and the time
            float rotateStep = rotationSpeed * Time.deltaTime;

            // Calculate the new direction by rotating towards the target
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget, rotateStep, 0f);

            // Apply the new direction as the forward direction of the projectile
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Check if the projectile collided with an object tagged "Dragon"
        if (other.CompareTag("Enemy")) {
            // Instantiate the explosion effect at the projectile's position and rotation
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            Debug.Log("hit");
            dragon.TakeHit();
            // Destroy the projectile
            Destroy(gameObject);
        }
        else {
            if (other.CompareTag("Player")) {
                rb.isKinematic = false;
            }
        }
    }
}
