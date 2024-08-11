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
    [SerializeField] public Transform damageNumPosition;

    protected float currentHealth;

    protected static readonly int hurtHash = Animator.StringToHash("Hurt");

    [SerializeField] protected SphereCollider detectCollider;
    public Animator animator;
    protected Rigidbody rb;

    protected float currentVelocity;
    protected bool isMoving;

    [SerializeField] protected GameObject smokeVFX;
    //public ManagerHolder managerHolder;
    protected UltimateTextDamageManager damageNumber;

    public bool isDead = false;

    private void Awake() {
        
    }
    private void Start() {
        //damageNumber = FindObjectOfType<UltimateTextDamageManager>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        //managerHolder = ManagerHolder.Instance;
    }
    public virtual void TakeDamage(float damage) {
        currentHealth -= damage;
        string roundedDamage = Mathf.Round(damage).ToString(); // Rounds to the nearest integer
        damageNumber = FindObjectOfType<UltimateTextDamageManager>();
        damageNumber.Add(roundedDamage, damageNumPosition, "default");
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
            animator.enabled = true;
        }
        isDead = true;
        Destroy(gameObject, 1.5f);
    }

    protected virtual void Suspension() {
        StartCoroutine(SuspensionEffect());
    }


    protected abstract void Move();
    protected abstract void Attack();

    private IEnumerator SuspensionEffect() {
        float originalMoveSpeed = moveSpeed;
        moveSpeed = 0.3f;
        yield return new WaitForSeconds(3);
        moveSpeed = originalMoveSpeed;
    }
}