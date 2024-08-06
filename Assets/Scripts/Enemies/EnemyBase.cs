using Guirao.UltimateTextDamage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] public float minDamage;
    [SerializeField] public float maxDamage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] public UltimateTextDamageManager damageNumberManager;
    [SerializeField] public Transform damageNumPosition;

    protected float currentHealth;

    protected static readonly int hurtHash = Animator.StringToHash("Hurt");

    [SerializeField] protected SphereCollider detectCollider;
    public Animator animator;
    protected Rigidbody rb;

    protected float currentVelocity;
    protected bool isMoving;

    [SerializeField] protected GameObject smokeVFX;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    public virtual void TakeDamage(float damage) {
        currentHealth -= damage;
        string roundedDamage = Mathf.Round(damage).ToString(); // Rounds to the nearest integer
        damageNumberManager.Add(roundedDamage, damageNumPosition, "default");
        Debug.Log(currentHealth);
        if (currentHealth <= 0) {
            currentHealth = 0;
            Death();
        }
        else {
            animator.SetTrigger(hurtHash);
        }
    }


    protected virtual void Death() {
        if (animator != null) {
            animator.Play("Death", 0, 0f);
            animator.enabled = false;
        }
        Destroy(gameObject, 1.5f);
    }


    protected abstract void Move();
    protected abstract void Attack();
}