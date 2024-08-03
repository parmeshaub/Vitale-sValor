using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float maxHealth = 10;
    public float health = 10f;
    public float damamage = 10f;
    public float walkSpeed = 2f;

    private void Start()
    {
        health = maxHealth;
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);


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
