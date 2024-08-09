using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementFunctionality : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider;
    [SerializeField] private float burnDamage = 100f;
    [SerializeField] private float burnInterval = 0.6f; // Time between each burn damage application

    private HashSet<EnemyBase> enemiesInTrigger = new HashSet<EnemyBase>();

    private void OnTriggerEnter(Collider other) {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null && !enemiesInTrigger.Contains(enemy)) {
            enemiesInTrigger.Add(enemy);
            StartCoroutine(BurnAttack(enemy));
        }
    }

    private void OnTriggerExit(Collider other) {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null && enemiesInTrigger.Contains(enemy)) {
            enemiesInTrigger.Remove(enemy);
        }
    }

    private IEnumerator BurnAttack(EnemyBase enemy) {
        while (enemiesInTrigger.Contains(enemy)) {
            if (!enemy.isDead) {
                enemy.TakeDamage(burnDamage);
            }
            yield return new WaitForSeconds(burnInterval);
        }
    }
}
