using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RazorFangsFunctionalityScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float damage = 60;
    private void Start() {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitBeforeDelete());
    }

    private void OnCollisionEnter(Collision collision) {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (collision.gameObject.layer == enemyLayer) {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (rb != null) {
                // Define the impulse force
                Vector3 impulseForce = new Vector3(0, 7, -10); // Adjust the values as needed
                // Apply the impulse force
                rb.AddForce(impulseForce, ForceMode.Impulse);
                enemy.TakeDamage(damage);
            }
        }
    }

    private IEnumerator WaitBeforeDelete() {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
