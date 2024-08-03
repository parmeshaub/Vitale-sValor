using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider heavyAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

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
            float damage = 0;

            Debug.Log("Heavy attack triggered");
            damage = playerCombat.HeavyRandomizeDamage(); // Adjust the damage calculation as needed

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
