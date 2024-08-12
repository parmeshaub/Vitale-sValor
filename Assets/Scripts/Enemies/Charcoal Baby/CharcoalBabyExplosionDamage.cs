using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcoalBabyExplosionDamage : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider;
    private PlayerHealthAndDamage playerHealth;
    private GameObject playerObject;
    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;

    private void Start() {
        StartCoroutine(DestroySelf());  
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            playerObject = other.gameObject;
            playerHealth = playerObject.GetComponent<PlayerHealthAndDamage>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(RandomizeDamage());
                Destroy(attackCollider);
            }

        }
    }

    private float RandomizeDamage() {
        float damage = Random.Range(minDamage, maxDamage);
        return damage;
    }

    public void TurnAttackColliderOn() {
        attackCollider.enabled = true;
    }

    public void TurnAttackColliderOff() {
        attackCollider.enabled = false;
    }
}
