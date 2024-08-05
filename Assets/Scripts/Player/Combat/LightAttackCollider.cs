using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider lightAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private GameObject normalHitVFXPrefab;

    private void Start()
    {
        lightAttackCollider.enabled = false; //Make sure that collider is off.
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("run");
        // Get the EnemyClass component from the collided object.
        EnemyClass enemyClass = other.GetComponent<EnemyClass>();

        if (enemyClass != null) {

            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Vector3 directionToPlayer = (transform.position - contactPoint).normalized;
            float offsetDistance = 0.2f; // Adjust this value as needed
            Vector3 offsetContactPoint = contactPoint + directionToPlayer * offsetDistance;

            if (normalHitVFXPrefab != null) {
                Instantiate(normalHitVFXPrefab, offsetContactPoint, Quaternion.identity);
            }

            float damage = playerCombat.LightRandomizeDamage();
            enemyClass.TakeDamage(damage);
        }
    }



    public void TurnLightAttackColliderOn()
    {
        lightAttackCollider.enabled = true;
    }

    public void TurnLightAttackColliderOff()
    {
        lightAttackCollider.enabled = false;
    }
}
