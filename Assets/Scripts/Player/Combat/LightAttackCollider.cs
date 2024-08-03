using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider lightAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

    private void Start()
    {
        lightAttackCollider.enabled = false; //Make sure that collider is off.
    }
    private void OnTriggerEnter(Collider other)
    {
        // Get the EnemyClass component from the collided object.
        EnemyClass enemyClass = other.GetComponent<EnemyClass>();
        if (enemyClass != null)
        {
            float damage = 0;

            Debug.Log("Light attack triggered");
            damage = playerCombat.LightRandomizeDamage(); // Adjust the damage calculation as needed

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
