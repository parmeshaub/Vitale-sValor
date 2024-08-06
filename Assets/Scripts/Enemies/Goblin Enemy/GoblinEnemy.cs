using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GoblinEnemy : EnemyBase
{
    // Animation Hashes
    private static readonly int attack01_Hash = Animator.StringToHash("Attack_01");
    private static readonly int attack02_Hash = Animator.StringToHash("Attack_02");
    private static readonly int death01_Hash = Animator.StringToHash("Death_01");
    private static readonly int death02_Hash = Animator.StringToHash("Death_02");
    private static readonly int jumpAttack_Hash = Animator.StringToHash("Jump_Attack");
    private static readonly int velocityHash = Animator.StringToHash("Velocity");
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");

    [SerializeField] private LayerMask deathLayerMask;
    [SerializeField] private CapsuleCollider capCollider;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float waitingDistance = 5f; // Distance to wait while another goblin is attacking
    [SerializeField] private float smoothTime = 0.3f; // Smoothing time for movement
    private float detectRange = 5;

    private GoblinStates states;
    private PlayerCombat playerCombat;
    private BattleSphereDetection enemyDetection;

    private Coroutine waitBeforePlayerLeaves;
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
        states = GoblinStates.IDLE;

        currentHealth = maxHealth;
        Debug.Log(currentHealth);
    }

    private void OnTriggerEnter(Collider other) {

        playerObject = playerCombat.gameObject;
        if (waitBeforePlayerLeaves != null) {
            StopCoroutine(waitBeforePlayerLeaves);
            waitBeforePlayerLeaves = null;
        }
        states = GoblinStates.MOVING;

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
                states = GoblinStates.IDLE;
            }
        }
    }

    private IEnumerator StateMachineCoroutine() {
        while (true) {
            switch (states) {
                case GoblinStates.IDLE:
                    isMoving = false;
                    break;
                case GoblinStates.MOVING:
                    isMoving = true;
                    Move();
                    break;
                case GoblinStates.ATTACK:
                    isMoving = false;
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(PerformAttack());
                    }
                    break;
            }

            // Check the distance to the player if moving
            if (states == GoblinStates.MOVING && playerObject != null) {
                float distToPlayer = CheckDistanceFromPlayer(playerObject);
                if (distToPlayer <= attackRange) {
                    states = GoblinStates.ATTACK;
                }
            }

            HandleAnimation();
            yield return null; // Wait until the next frame
        }
    }

    protected override void Move() {
        if (isDeath) return;
        // Walk towards the player. Uses a rigid body.

        // Calculate the direction towards the player, ignoring the Y-axis
        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);

        // Rotate to face the player without changing the Y-axis
        transform.DOLookAt(targetPosition, 0.2f);

        // Calculate the direction towards the player
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Move the goblin towards the player
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    protected override void Attack() {
        if (attackCoroutine == null) {
            rb.angularVelocity = Vector3.zero;
            attackCoroutine = StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack() {
        // Implement attack logic
        int random = Random.Range(0, 2);
        switch (random) {
            case 0:
                animator.SetTrigger(attack01_Hash);
                break;
            case 1:
                animator.SetTrigger(attack02_Hash);
                break;
        }

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(3f);

        // Check if the playerObject is still valid
        if (playerObject != null) {
            float distToPlayer = CheckDistanceFromPlayer(playerObject);
            if (distToPlayer <= attackRange) {
                states = GoblinStates.ATTACK;
            }
            else {
                states = GoblinStates.MOVING;
            }
        }
        else {
            states = GoblinStates.IDLE;
        }

        attackCoroutine = null;
    }

    private float CheckDistanceFromPlayer(GameObject playerObject) {
        return Vector3.Distance(transform.position, playerObject.transform.position);
    }

    private void HandleAnimation() {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float speed = horizontalVelocity.magnitude / moveSpeed; // Normalize by moveSpeed
        animator.SetFloat(velocityHash, speed);
        animator.SetBool(isMovingHash, isMoving);
    }

    protected override void Death() {
        enemyDetection.RemoveEnemy(this.gameObject);
        gameObject.layer = deathLayerMask;
        capCollider.isTrigger = true;

        int random = Random.Range(0, 2);
        isDeath = true;

        StartCoroutine(WaitBeforeDeath(random));
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        states = GoblinStates.IDLE; // Add this line to handle state change on damage
        ApplyImpulseBackwards();
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

    private IEnumerator WaitBeforeDeath(int random) {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y - 0.399f, transform.position.z);
        transform.DOMove(newPosition, 2);
        switch (random) {
            case 0:
                animator.enabled = false;
                yield return new WaitForSeconds(0.05f);
                animator.enabled = true;
                animator.SetTrigger(death01_Hash);
                break;
            case 1:
                animator.enabled = false;
                yield return new WaitForSeconds(0.05f);
                animator.enabled = true;
                animator.SetTrigger(death02_Hash);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(2f);

        // Instantiate the smokeVFX at the current position and rotation
        Instantiate(smokeVFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}

public enum GoblinStates
{
    IDLE,
    MOVING,
    ATTACK,
    FIND_PLAYER
}
