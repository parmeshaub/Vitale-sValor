using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float damamage = 10f;
    public float walkSpeed = 2f;

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void Attack()
    {

    }
}
