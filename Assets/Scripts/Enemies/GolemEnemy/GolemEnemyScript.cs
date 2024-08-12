using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GolemEnemyScript : EnemyBase
{
    // Animation Hashes
    private static readonly int smashHash = Animator.StringToHash("Smash");
    private static readonly int deathHash = Animator.StringToHash("Death");
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");

    [SerializeField] private LayerMask deathLayerMask;
    [SerializeField] private CapsuleCollider capCollider;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float smoothTime = 0.3f; // Smoothing time for movement
    private float detectRange = 5f;

    private GolemStates states;
    private PlayerCombat playerCombat;
    private BattleSphereDetection enemyDetection;

    private Coroutine stateMachineCoroutine;
    private Coroutine attackCoroutine;

    private GameObject playerObject;

    private bool isDeath;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        capCollider = GetComponent<CapsuleCollider>();
    }

    private void Start() {
        playerCombat = PlayerCombat.Instance;
        enemyDetection = playerCombat.battleSphereDetection;
        states = GolemStates.IDLE;

        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other) {

        playerObject = playerCombat.gameObject;
        states = GolemStates.MOVING;

        // Assign the player
        playerObject = other.gameObject;

        // Start the state machine coroutine
        if (stateMachineCoroutine == null) {
            stateMachineCoroutine = StartCoroutine(StateMachineCoroutine());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            // Check the distance before setting playerObject to null
            if (CheckDistanceFromPlayer(other.gameObject) > detectRange) {
                playerObject = null;
                Debug.Log("Exit");
                states = GolemStates.IDLE;
            }
        }
    }

    private IEnumerator StateMachineCoroutine() {
        while (true) {
            if (isDeath) yield break; // Stop the coroutine if the golem is dead

            switch (states) {
                case GolemStates.IDLE:
                    isMoving = false;
                    break;
                case GolemStates.MOVING:
                    isMoving = true;
                    Move();
                    break;
                case GolemStates.ATTACK:
                    isMoving = false;
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(PerformAttack());
                    }
                    break;
            }

            // Check the distance to the player if moving
            if (states == GolemStates.MOVING && playerObject != null) {
                float distToPlayer = CheckDistanceFromPlayer(playerObject);
                if (distToPlayer <= attackRange) {
                    states = GolemStates.ATTACK;
                }
            }

            HandleAnimation();
            yield return null; // Wait until the next frame
        }
    }

    protected override void Move() {
        if (isDeath) return;

        // Calculate the direction towards the player, ignoring the Y-axis
        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);

        // Rotate to face the player without changing the Y-axis
        transform.DOLookAt(targetPosition, 0.2f);

        // Calculate the direction towards the player
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the golem towards the player
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    protected override void Attack() {
        if (isDeath || isDead) return;
        if (attackCoroutine == null) {
            rb.angularVelocity = Vector3.zero;
            attackCoroutine = StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack() {
        if (isDeath) yield break; // Stop the coroutine if the golem is dead

        // Implement attack logic
        animator.SetTrigger(smashHash);

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(3f);

        // Check if the playerObject is still valid
        if (playerObject != null) {
            float distToPlayer = CheckDistanceFromPlayer(playerObject);
            if (distToPlayer <= attackRange) {
                states = GolemStates.ATTACK;
            }
            else {
                states = GolemStates.MOVING;
            }
        }
        else {
            states = GolemStates.IDLE;
        }

        attackCoroutine = null;
    }

    private float CheckDistanceFromPlayer(GameObject playerObject) {
        return Vector3.Distance(transform.position, playerObject.transform.position);
    }

    private void HandleAnimation() {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float speed = horizontalVelocity.magnitude / moveSpeed; // Normalize by moveSpeed
        animator.SetBool(isMovingHash, isMoving);
    }

    protected override void Death() {
        if (isDeath) return; // Prevent multiple death calls

        isDeath = true;
        CallDeath();
        enemyDetection.RemoveEnemy(this.gameObject);
        gameObject.layer = deathLayerMask;
        capCollider.isTrigger = true;

        isDead = true;

        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        // Stop any moving animation by setting isMoving to false
        isMoving = false;
        animator.SetBool(isMovingHash, isMoving);

        StartCoroutine(WaitBeforeDeath());
    }

    private void CallDeath() {
        animator.enabled = false;
        StartCoroutine(WaitEnableAnimator());
    }

    private IEnumerator WaitEnableAnimator() {
        yield return new WaitForSeconds(0.2f);
        animator.enabled = true;
    }

    private IEnumerator WaitBeforeDeath() {
        rb.angularVelocity = Vector3.zero; // Instantly stops spinning
        yield return new WaitForSeconds(0.2f);
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - 0.299f, transform.position.z);
        transform.DOMove(newPosition, 2);
        animator.enabled = false;
        yield return new WaitForSeconds(0.05f);
        animator.enabled = true;
        animator.SetTrigger(deathHash);
        yield return new WaitForSeconds(2f);

        // Instantiate the smokeVFX at the current position and rotation
        Instantiate(smokeVFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        if (!isDeath) {
            states = GolemStates.IDLE; // Add this line to handle state change on damage
            ApplyImpulseBackwards();
        }
        rb.angularVelocity = Vector3.zero; // Instantly stops spinning
    }

    private void ApplyImpulseBackwards() {
        // Check if the Rigidbody is assigned
        if (rb != null) {
            // Calculate the backward direction (opposite of the forward direction)
            Vector3 backwardDirection = -transform.forward;

            // Apply an impulse force in the backward direction
            float impulseForce = 2f; // Adjust this value as needed
            rb.AddForce(backwardDirection * impulseForce, ForceMode.Impulse);
        }
    }
}