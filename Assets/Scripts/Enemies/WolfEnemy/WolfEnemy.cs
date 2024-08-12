using DG.Tweening;
using System.Collections;
using UnityEngine;

public class WolfEnemy : EnemyBase
{
    // Animation Hashes
    private static readonly int hurtHash = Animator.StringToHash("Hurt");
    private static readonly int deathHash = Animator.StringToHash("Death");
    private static readonly int biteHash = Animator.StringToHash("Bite");
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");

    [SerializeField] private LayerMask deathLayerMask;
    [SerializeField] private CapsuleCollider capCollider;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float smoothTime = 0.3f;
    private float detectRange = 7f;

    private WolfStates states;
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
        states = WolfStates.IDLE;

        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other) {
        playerObject = playerCombat.gameObject;

        states = WolfStates.MOVING;

        // Assign the player
        playerObject = other.gameObject;

        // Start the state machine coroutine
        if (stateMachineCoroutine == null) {
            stateMachineCoroutine = StartCoroutine(StateMachineCoroutine());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (CheckDistanceFromPlayer(other.gameObject) > detectRange) {
                playerObject = null;
                states = WolfStates.IDLE;
            }
        }
    }

    private IEnumerator StateMachineCoroutine() {
        while (true) {
            if (isDeath) yield break;

            switch (states) {
                case WolfStates.IDLE:
                    isMoving = false;
                    break;
                case WolfStates.MOVING:
                    isMoving = true;
                    Move();
                    break;
                case WolfStates.ATTACK:
                    isMoving = false;
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(PerformAttack());
                    }
                    break;
            }

            if (states == WolfStates.MOVING && playerObject != null) {
                float distToPlayer = CheckDistanceFromPlayer(playerObject);
                if (distToPlayer <= attackRange) {
                    states = WolfStates.ATTACK;
                }
            }

            HandleAnimation();
            yield return null;
        }
    }

    protected override void Move() {
        if (isDeath) return;

        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);
        transform.DOLookAt(targetPosition, 0.2f);

        Vector3 direction = (targetPosition - transform.position).normalized;

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
        if (isDeath) yield break;

        // Trigger the bite attack animation
        animator.SetTrigger(biteHash);

        // Adjust the wait time to a shorter duration
        yield return new WaitForSeconds(1f); // Reduced delay

        if (playerObject != null) {
            float distToPlayer = CheckDistanceFromPlayer(playerObject);
            if (distToPlayer <= attackRange) {
                states = WolfStates.ATTACK;
            }
            else {
                states = WolfStates.MOVING;
            }
        }
        else {
            states = WolfStates.IDLE;
        }

        attackCoroutine = null;
    }


    private float CheckDistanceFromPlayer(GameObject playerObject) {
        return Vector3.Distance(transform.position, playerObject.transform.position);
    }

    private void HandleAnimation() {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float speed = horizontalVelocity.magnitude / moveSpeed;
        animator.SetBool(isMovingHash, isMoving);
    }

    protected override void Death() {
        if (isDeath) return;

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

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        if (!isDeath) {
            states = WolfStates.IDLE;
            animator.SetTrigger(hurtHash);
            ApplyImpulseBackwards();
        }
        rb.angularVelocity = Vector3.zero;
    }

    private void ApplyImpulseBackwards() {
        if (rb != null) {
            Vector3 backwardDirection = -transform.forward;
            float impulseForce = 2f;
            rb.AddForce(backwardDirection * impulseForce, ForceMode.Impulse);
        }
    }

    private IEnumerator WaitBeforeDeath() {
        rb.angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.DOMove(newPosition, 2);
        animator.SetTrigger(deathHash);

        yield return new WaitForSeconds(1.3f);

        Instantiate(smokeVFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}

public enum WolfStates
{
    IDLE,
    MOVING,
    ATTACK
}
