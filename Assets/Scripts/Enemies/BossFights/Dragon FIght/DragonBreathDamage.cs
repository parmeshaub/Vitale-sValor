using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathDamage : MonoBehaviour
{ 
    private GameObject player;
    private PlayerHealthAndDamage playerHealth;

    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealthAndDamage>();
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Hit Player");
            playerHealth.TakeDamage(RandomizeDamage());
        }
    }


    private float RandomizeDamage() {
        float damage = Random.Range(minDamage, maxDamage);
        return damage;
    }
}
