using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BattleSphereDetection : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    public Transform FindClosestEnemy()
    {
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (!enemy.IsDestroyed())
            {
                Vector3 directionToEnemy = enemy.transform.position - currentPosition;
                float dSqrToEnemy = directionToEnemy.sqrMagnitude;
                if (dSqrToEnemy < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToEnemy;
                    closestEnemy = enemy.transform;
                }
            }
           
        }

        return closestEnemy;
    }
}
