using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSphereDetection : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public Transform FindClosestEnemy() {
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        // Iterate over the list of enemies and find the closest one
        for (int i = enemiesInRange.Count - 1; i >= 0; i--) {
            GameObject enemy = enemiesInRange[i];

            // If the enemy is destroyed or its layer is no longer "Enemy", remove it
            if (enemy.IsDestroyed() || enemy.layer != LayerMask.NameToLayer("Enemy")) {
                enemiesInRange.RemoveAt(i);
                continue;
            }

            Vector3 directionToEnemy = enemy.transform.position - currentPosition;
            float dSqrToEnemy = directionToEnemy.sqrMagnitude;
            if (dSqrToEnemy < closestDistanceSqr) {
                closestDistanceSqr = dSqrToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    public void RemoveEnemy(GameObject enemyToRemove) {
        if (enemiesInRange.Contains(enemyToRemove)) {
            enemiesInRange.Remove(enemyToRemove);
            Debug.Log("Enemy successfully removed: " + enemyToRemove.name);
        }
        else {
            Debug.LogWarning("Attempted to remove enemy not in list: " + enemyToRemove.name);
        }
    }
}
