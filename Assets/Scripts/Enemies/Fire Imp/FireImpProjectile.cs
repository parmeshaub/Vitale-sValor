using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireImpProjectile : MonoBehaviour
{
    private EnemyBase enemyBase;
    private void Start() {
        enemyBase = GetComponentInParent<EnemyBase>();
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            Debug.Log("Hit PLayer");
            Destroy(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
