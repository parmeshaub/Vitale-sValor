using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SkeletonEnemy : EnemyBase
{
    // Animation Hashes
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int hurtHash = Animator.StringToHash("Hurt");
    private static readonly int death1Hash = Animator.StringToHash("Death1");
    private static readonly int death2Hash = Animator.StringToHash("Death2");
    private static readonly int attack01Hash = Animator.StringToHash("Attack01");
    private static readonly int attack02Hash = Animator.StringToHash("Attack02");
    private static readonly int attack03Hash = Animator.StringToHash("Attack03");

    [SerializeField] private LayerMask deathLayerMask;
    [SerializeField] private CapsuleCollider capCollider;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float smoothTime = 0.3f;
    private float detectRange = 7f;

    private SkeletonStates states;
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
        states = SkeletonStates.IDLE;

        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other) {
        // Only assign the playerObject if the collider belongs to the player
        if (other.CompareTag("Player")) {
            playerObject = playerCombat.gameObject;
            states = SkeletonStates.MOVING;

            if (stateMachineCoroutine == null) {
                stateMachineCoroutine = StartCoroutine(StateMachineCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (CheckDistanceFromPlayer(other.gameObject) > detectRange) {
                playerObject = null;
                states = SkeletonStates.IDLE;
            }
        }
    }


    private IEnumerator StateMachineCoroutine() {
        while (true) {
            if (isDeath) yield break;

            switch (states) {
                case SkeletonStates.IDLE:
                    isMoving = false;
                    break;
                case SkeletonStates.MOVING:
                    isMoving = true;
                    Move();
                    break;
                case SkeletonStates.ATTACK:
                    isMoving = false;
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(PerformAttack());
                    }
                    break;
            }

            if (states == SkeletonStates.MOVING && playerObject != null) {
                float distToPlayer = CheckDistanceFromPlayer(playerObject);
                if (distToPlayer <= attackRange) {
                    states = SkeletonStates.ATTACK;
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

        int random = Random.Range(0, 3);
        switch (random) {
            case 0:
                animator.SetTrigger(attack01Hash);
                break;
            case 1:
                animator.SetTrigger(attack02Hash);
                break;
            case 2:
                animator.SetTrigger(attack03Hash);
                break;
        }

        yield return new WaitForSeconds(3f);

        if (playerObject != null) {
            float distToPlayer = CheckDistanceFromPlayer(playerObject);
            if (distToPlayer <= attackRange) {
                states = SkeletonStates.ATTACK;
            }
            else {
                states = SkeletonStates.MOVING;
            }
        }
        else {
            states = SkeletonStates.IDLE;
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
            states = SkeletonStates.IDLE;
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
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y+0.4f, transform.position.z);
        transform.DOMove(newPosition, 0.05f);
        yield return new WaitForSeconds(0.1f);
        Vector3 newpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.DOMove(newpos, 5.5f);

        int random = Random.Range(0, 2);
        switch (random) {
            case 0:
                animator.enabled = false;
                yield return new WaitForSeconds(0.05f);
                animator.enabled = true;
                animator.SetTrigger(death1Hash);
                break;
            case 1:
                animator.enabled = false;
                yield return new WaitForSeconds(0.05f);
                animator.enabled = true;
                animator.SetTrigger(death2Hash);
                break;
        }

        yield return new WaitForSeconds(3f);

        Instantiate(smokeVFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}

public enum SkeletonStates
{
    IDLE,
    MOVING,
    ATTACK
}
