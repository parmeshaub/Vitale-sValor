using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider lightAttackCollider;
    [SerializeField] private BoxCollider heavyAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

    private void Start()
    {
        heavyAttackCollider.enabled = false;
        lightAttackCollider.enabled = false; //Make sure that collider is off.
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        // Get the EnemyClass component from the collided object.
        EnemyClass enemyClass = other.GetComponent<EnemyClass>();
        if (enemyClass != null)
        {
            float damage = 0;

            if (lightAttackCollider.enabled && lightAttackCollider.bounds.Intersects(other.bounds))
            {
                // Light attack logic
                Debug.Log("Light attack triggered");
                damage = playerCombat.RandomizeDamage(); // Adjust the damage calculation as needed
            }
            else if (heavyAttackCollider.enabled && heavyAttackCollider.bounds.Intersects(other.bounds))
            {
                // Heavy attack logic
                Debug.Log("Heavy attack triggered");
                damage = playerCombat.RandomizeDamage() * 1.5f; // Adjust the damage calculation as needed
            }

            enemyClass.TakeDamage(damage);
        }

        public void TurnAttackColliderOn()
    {
        attackCollider.enabled = true;
    }

    public void TurnAttackColliderOff()
    {
        attackCollider.enabled = false;
    }
*/
}
