using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider;
    private PlayerHealthAndDamage playerHealth;
    private GameObject playerObject;
    [SerializeField] private EnemyBase parentEnemy;


    private void Start() {
        attackCollider.enabled = false;
        if(parentEnemy == null) {
            parentEnemy = GetComponentInParent<EnemyBase>();
        }

    }

    private void OnTriggerEnter(Collider other) {
        if(other != null) {
            playerObject = other.gameObject;
            playerHealth = playerObject.GetComponent<PlayerHealthAndDamage>();
            if(playerHealth != null) {
                playerHealth.TakeDamage(RandomizeDamage());
            }
        }
    }

    private float RandomizeDamage() {
        float damage = Random.Range(parentEnemy.minDamage, parentEnemy.maxDamage);
        return damage;
    }

    public void TurnAttackColliderOn() {
        attackCollider.enabled = true;
    }

    public void TurnAttackColliderOff() {
        attackCollider.enabled = false;
    }
}
