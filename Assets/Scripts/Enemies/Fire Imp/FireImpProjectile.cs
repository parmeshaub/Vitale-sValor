using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireImpProjectile : MonoBehaviour
{
    private EnemyBase enemyBase;
    [SerializeField] private GameObject impactVFX;
    private GameObject player;
    private PlayerHealthAndDamage playerHealth;

    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;

    private void Start() {
        enemyBase = GetComponentInParent<EnemyBase>();
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealthAndDamage>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Hit Player");
            Instantiate(impactVFX, transform.position, Quaternion.identity);
            playerHealth.TakeDamage(RandomizeDamage());
            Destroy(gameObject);
        }
        else {
            Instantiate(impactVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private float RandomizeDamage() {
        float damage = Random.Range(minDamage, maxDamage);
        
        return damage;
    }
}
