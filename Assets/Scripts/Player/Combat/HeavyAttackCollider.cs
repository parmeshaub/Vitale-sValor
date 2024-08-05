using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider heavyAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private GameObject normalHitVFXPrefab;

    private void Start()
    {
        heavyAttackCollider.enabled = false; //Make sure that collider is off.
    }
    private void OnTriggerEnter(Collider other)
    {
        // Get the EnemyClass component from the collided object.
        EnemyClass enemyClass = other.GetComponent<EnemyClass>();
        if (enemyClass != null)
        {
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            Instantiate(normalHitVFXPrefab, contactPoint, Quaternion.identity);

            float damage = 0;

            Debug.Log("Heavy attack triggered");
            damage = playerCombat.HeavyRandomizeDamage();

            enemyClass.TakeDamage(damage);

            
        }
    }

    public void TurnHeavyAttackColliderOn()
    {
        heavyAttackCollider.enabled = true;
    }

    public void TurnHeavyAttackColliderOff()
    {
        heavyAttackCollider.enabled = false;
    }
}
