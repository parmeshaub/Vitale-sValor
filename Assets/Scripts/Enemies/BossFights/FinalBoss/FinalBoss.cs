using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : EnemyBase
{
    // Animation Hashes
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int attack01Hash = Animator.StringToHash("Attack01");
    private static readonly int attack02Hash = Animator.StringToHash("Attack02");
    private static readonly int attack03Hash = Animator.StringToHash("Attack03");
    private static readonly int attack04Hash = Animator.StringToHash("Attack04");
    private static readonly int deathHash = Animator.StringToHash("Death");

    [SerializeField] private LayerMask deathLayerMask;
    [SerializeField] private CapsuleCollider capCollider;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float smoothTime = 0.3f;
    private float detectRange = 10f;

    private BossStates states;
    private PlayerCombat playerCombat;
    private BattleSphereDetection enemyDetection;

    private Coroutine stateMachineCoroutine;
    private Coroutine attackCoroutine;

    private GameObject playerObject;

    private bool isDeath;

    // Custom wait times for each attack
    private Dictionary<int, float> attackWaitTimes = new Dictionary<int, float>
    {
        { 0, 2f },  // Attack01
        { 1, 3f },  // Attack02
        { 2, 6f },  // Attack03
        { 3, 2.5f }  // Attack04
    };

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        capCollider = GetComponent<CapsuleCollider>();
    }

    private void Start() {
        playerCombat = PlayerCombat.Instance;
        enemyDetection = playerCombat.battleSphereDetection;
        states = BossStates.IDLE;

        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerObject = playerCombat.gameObject;
            if (states == BossStates.IDLE) {
                states = BossStates.MOVING;
            }

            if (stateMachineCoroutine == null) {
                stateMachineCoroutine = StartCoroutine(StateMachineCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (CheckDistanceFromPlayer(other.gameObject) > detectRange) {
                playerObject = null;
                states = BossStates.IDLE;
            }
        }
    }

    private IEnumerator StateMachineCoroutine() {
        while (true) {
            if (isDeath) yield break;

            if (playerObject == null) {
                states = BossStates.IDLE;
            }

            switch (states) {
                case BossStates.IDLE:
                    isMoving = false;
                    CheckForPlayerProximity();
                    break;
                case BossStates.MOVING:
                    isMoving = true;
                    Move();
                    break;
                case BossStates.ATTACK:
                    isMoving = false;
                    if (attackCoroutine == null) {
                        attackCoroutine = StartCoroutine(PerformAttack());
                    }
                    break;
            }

            HandleAnimation();
            yield return null;
        }
    }

    private void CheckForPlayerProximity() {
        if (playerObject != null) {
            float distToPlayer = CheckDistanceFromPlayer(playerObject);
            if (distToPlayer <= detectRange) {
                states = BossStates.MOVING;
            }
        }
    }

    protected override void Move() {
        if (isDeath || playerObject == null) return;

        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);
        transform.DOLookAt(targetPosition, 0.2f);

        Vector3 direction = (targetPosition - transform.position).normalized;

        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        // Ensure we transition to ATTACK state when in range
        float distToPlayer = CheckDistanceFromPlayer(playerObject);
        if (distToPlayer <= attackRange) {
            states = BossStates.ATTACK;
        }
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

        // Make the boss look at the player before attacking (ignore Y axis)
        if (playerObject != null) {
            Vector3 lookPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);
            transform.DOLookAt(lookPosition, 0.2f);  // Adjust the duration as needed
        }

        yield return new WaitForSeconds(0.2f);  // Wait for the look-at to complete

        int random = Random.Range(0, 4);
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
            case 3:
                animator.SetTrigger(attack04Hash);
                break;
        }

        // Wait for the custom duration of the attack
        yield return new WaitForSeconds(attackWaitTimes[random]);

        if (playerObject != null) {
            float distToPlayer = CheckDistanceFromPlayer(playerObject);
            if (distToPlayer <= attackRange) {
                Debug.Log("back to Attack?");
                states = BossStates.ATTACK;
            }
            else {
                Debug.Log("back to move?");
                states = BossStates.MOVING;
            }
        }
        else {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            states = BossStates.MOVING;
        }

        attackCoroutine = null;
    }

    private float CheckDistanceFromPlayer(GameObject playerObject) {
        return Vector3.Distance(transform.position, playerObject.transform.position);
    }

    private void HandleAnimation() {
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
            states = BossStates.IDLE;
            ApplyImpulseBackwards();
            CheckForPlayerProximity(); // Check if the player is close after taking damage
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
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);
        transform.DOMove(newPosition, 0.05f);
        yield return new WaitForSeconds(0.1f);
        Vector3 newpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.DOMove(newpos, 5.5f);

        animator.enabled = false;
        yield return new WaitForSeconds(0.05f);
        animator.enabled = true;
        animator.SetTrigger(deathHash);

        yield return new WaitForSeconds(3f);

        Instantiate(smokeVFX, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}

public enum BossStates
{
    IDLE,
    MOVING,
    ATTACK
}
