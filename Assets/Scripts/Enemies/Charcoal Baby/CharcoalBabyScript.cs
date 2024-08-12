using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharcoalBabyScript : EnemyBase
{
    // Animation Hashes
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int attackHash = Animator.StringToHash("Attack");

    [SerializeField] private LayerMask deathLayerMask;
    [SerializeField] private CapsuleCollider capCollider;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float smoothTime = 0.3f;
    private float detectRange = 7f;

    private CharcoalBabyStates states;
    private PlayerCombat playerCombat;
    private BattleSphereDetection enemyDetection;

    private Coroutine stateMachineCoroutine;
    private Coroutine attackCoroutine;

    private GameObject playerObject;

    private bool isDeath;

    [SerializeField] private GameObject explosionVFX;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        capCollider = GetComponent<CapsuleCollider>();
    }

    private void Start() {
        playerCombat = PlayerCombat.Instance;
        enemyDetection = playerCombat.battleSphereDetection;
        states = CharcoalBabyStates.IDLE;

        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerObject = playerCombat.gameObject;
            states = CharcoalBabyStates.MOVING;

            if (stateMachineCoroutine == null) {
                stateMachineCoroutine = StartCoroutine(StateMachineCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (CheckDistanceFromPlayer(other.gameObject) > detectRange) {
                playerObject = null;
                states = CharcoalBabyStates.IDLE;
            }
        }
    }

    private IEnumerator StateMachineCoroutine() {
        while (true) {
            if (isDeath) yield break;

            switch (states) {
                case CharcoalBabyStates.IDLE:
                    isMoving = false;
                    break;
                case CharcoalBabyStates.MOVING:
                    isMoving = true;
                    Move();
                    break;
                case CharcoalBabyStates.ATTACK:
                    isMoving = false;
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(PerformAttack());
                    }
                    break;
            }

            if (states == CharcoalBabyStates.MOVING && playerObject != null) {
                float distToPlayer = CheckDistanceFromPlayer(playerObject);
                if (distToPlayer <= attackRange) {
                    states = CharcoalBabyStates.ATTACK;
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

        // Trigger the attack animation
        animator.SetTrigger(attackHash);

        // Wait for the attack animation to complete
        yield return new WaitForSeconds(0.4f);

        Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z); 
        Instantiate(explosionVFX, newPos, transform.rotation);

        // Destroy the Charcoal Baby after explosion
        Destroy(gameObject);
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

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        if (!isDeath) {
            states = CharcoalBabyStates.IDLE;
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
        yield return new WaitForSeconds(3f);

        Instantiate(smokeVFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}

public enum CharcoalBabyStates
{
    IDLE,
    MOVING,
    ATTACK
}
