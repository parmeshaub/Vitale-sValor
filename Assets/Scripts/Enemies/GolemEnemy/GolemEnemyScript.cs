using UnityEngine;
using System.Collections;
using DG.Tweening;
using Guirao.UltimateTextDamage;

public class GolemEnemyScript : EnemyBase
{
    private GolemStates currentState;
    private Transform playerTransform;
    private BattleSphereDetection enemyDetection;
    private PlayerCombat playerCombat;

    // Golem Stats
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldown = 5.0f;
    [SerializeField] private float detectionRadius = 5.0f; // Radius for detecting the player

    // Animation Hashes
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int attackHash = Animator.StringToHash("Smash");
    private static readonly int deathHash = Animator.StringToHash("Death");

    private CapsuleCollider capCollider;

    private Coroutine currentCoroutine;

    private bool playerInside = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start() {
        playerCombat = PlayerCombat.Instance;
        capCollider = GetComponent<CapsuleCollider>();
        enemyDetection = playerCombat.battleSphereDetection;
        currentState = GolemStates.Idle;
        TransitionToCoroutine(currentState);

        currentHealth = maxHealth;
    }

    private void TransitionToCoroutine(GolemStates newState) {
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }

        currentState = newState;

        switch (currentState) {
            case GolemStates.Idle:
                currentCoroutine = StartCoroutine(IdleState());
                break;
            case GolemStates.Find_Player:
                currentCoroutine = StartCoroutine(FindPlayerState());
                break;
            case GolemStates.Attack:
                currentCoroutine = StartCoroutine(AttackState());
                break;
            case GolemStates.Death:
                currentCoroutine = StartCoroutine(DeathState());
                break;
        }
    }

    private IEnumerator IdleState() {
        animator.SetBool(isMovingHash, false);

        while (true) {
            // The IdleState simply waits for the player to enter the detection radius,
            // which will be handled by the OnTriggerEnter method.
            yield return null; // Wait until the next frame
        }
    }

    private IEnumerator FindPlayerState() {
        Debug.Log("Entered FindPlayerState");
        animator.SetBool(isMovingHash, true);

        while (playerInside) {
            if (playerTransform != null) {
                // Calculate the direction towards the player, ignoring the Y-axis
                Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);

                // Rotate to face the player without changing the Y-axis
                transform.DOLookAt(targetPosition, 0.2f);

                // Calculate the direction towards the player
                Vector3 direction = (targetPosition - transform.position).normalized;

                // Move the Golem towards the player
                rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (distanceToPlayer <= attackRange) {
                    // Transition to Attack state if within range
                    TransitionToCoroutine(GolemStates.Attack);
                    yield break;
                }
            }
            else {
                // If playerTransform is null, exit to Idle state
                TransitionToCoroutine(GolemStates.Idle);
                yield break;
            }

            yield return null; // Continue execution in the next frame
        }

        // If player is no longer inside, exit to Idle state
        TransitionToCoroutine(GolemStates.Idle);
    }



    private IEnumerator AttackState() {
        animator.SetBool(isMovingHash, false);
        Debug.Log("Attack");
        // Attack animation and logic
        animator.SetTrigger(attackHash);
        yield return new WaitForSeconds(attackCooldown);

        // After attacking, check if player is still within range
        if (playerTransform != null) {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > attackRange) {
                // Return to finding player if out of attack range
                TransitionToCoroutine(GolemStates.Find_Player);
            }
            else {
                // Otherwise, continue attacking
                currentCoroutine = StartCoroutine(AttackState());
            }
        }
        else {
            // If player is not found, transition back to Idle
            TransitionToCoroutine(GolemStates.Idle);
        }
    }

    private IEnumerator DeathState() {
        animator.SetTrigger(deathHash);

        enemyDetection.RemoveEnemy(this.gameObject);
        gameObject.layer = LayerMask.NameToLayer("Default");
        capCollider.isTrigger = true;
        rb.isKinematic = true;

        // Perform the movement over 2 seconds, then destroy the object
        transform.DOMove(new Vector3(transform.position.x, transform.position.y - 0.899f, transform.position.z), 4)
            .OnComplete(() => {
                Debug.Log("Golem reached final position, preparing to destroy.");
                if (smokeVFX != null) {
                    Instantiate(smokeVFX, transform.position, transform.rotation);
                }
                else {
                    Debug.LogWarning("smokeVFX is not assigned.");
                }

                Destroy(gameObject);
                Debug.Log("Golem destroyed.");
            });

        yield return new WaitForSeconds(2f);

        // Coroutine ends here
        yield break;
    }



    public override void TakeDamage(float damage) {
        currentHealth -= damage;

        string roundedDamage = Mathf.Round(damage).ToString(); // Rounds to the nearest integer
        damageNumber = FindObjectOfType<UltimateTextDamageManager>();
        damageNumber.Add(roundedDamage, damageNumPosition, "default");

        Debug.Log(currentHealth);
        if (currentHealth <= 0) {
            currentHealth = 0;
            TransitionToCoroutine(GolemStates.Death);
        }
        else {
            animator.SetTrigger(hurtHash);
        }


    }

    protected override void Attack() {
        throw new System.NotImplementedException();
    }

    protected override void Move() {
        throw new System.NotImplementedException();
    }

    // This method is called when the player enters the Golem's detection range
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player Enter");
            playerInside = true;
            playerTransform = other.transform;
            // Transition to Find_Player state when the player enters detection range
            TransitionToCoroutine(GolemStates.Find_Player);
        }
    }

    // This method is called when the player exits the Golem's detection range
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInside = false;
            playerTransform = null;
            Debug.Log("Player Exit");
            // Transition back to Idle state when the player exits detection range
            TransitionToCoroutine(GolemStates.Idle);
        }
    }
}


public enum GolemStates
{
    Idle,
    Find_Player,
    Attack,
    Death
}