using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorilDamageOnce : MonoBehaviour
{
    [SerializeField] BoxCollider attackCollider;
    private PlayerHealthAndDamage playerHealth;
    private GameObject playerObject;
    private BorilScript borilScript;

    private void Start() {
        playerObject = GameObject.FindWithTag("Player");
        playerHealth = playerObject.GetComponent<PlayerHealthAndDamage>();
        borilScript = BorilScript.instance;
    }

    private void OnTriggerEnter(Collider other) {
        playerHealth.TakeDamage(RandomizeDamage());
        Destroy(attackCollider);
    }

    private float RandomizeDamage() {
        float damage = Random.Range(borilScript.minDamage, borilScript.maxDamage);
        return damage;
    }
}
