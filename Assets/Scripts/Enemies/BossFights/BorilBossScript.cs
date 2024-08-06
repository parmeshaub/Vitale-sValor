using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorilBossScript : EnemyBase
{
    private IceBossStates currentState;
    private IceBossPhases currentPhase;

    private PlayerCombat playerCombat;
    private GameObject playerObject;
    private float distanceThresholdToPlayer = 3;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;
    private int swipeCount = 0;
    private bool isCharging = false;

    private static readonly int isMovingHash = Animator.StringToHash("isMoving");


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        
    }

    private void Update() {
        animator.SetBool(isMovingHash,isMoving);
    }

    private void Start() {
        playerCombat = PlayerCombat.Instance;
        playerObject = playerCombat.gameObject;
        currentPhase = IceBossPhases.Waiting;
        StartIceBoss();
    }

    private void StartIceBoss() {
        currentPhase = IceBossPhases.Phase_Two;
        currentState = IceBossStates.Find_Player;
        SetState(currentState);
    }

    private void SetState(IceBossStates state) {
        currentState = state;
        StopAllCoroutines(); // Stop any running coroutine before starting a new one
        switch (state) {
            case IceBossStates.Find_Player:
                Debug.Log("finding player");
                StartCoroutine(FindPlayer());
                break;
            case IceBossStates.Idle:
                Debug.Log("idle");
                StartCoroutine(Idle());
                break;
            case IceBossStates.Ice_Breath:
                Debug.Log("ice");
                StartCoroutine(IceBreath());
                break;
            case IceBossStates.Charge:
                Debug.Log("charge");
                StartCoroutine(Charge());
                break;
            case IceBossStates.Swipe_Attack:
                Debug.Log("swipe");
                StartCoroutine(SwipeAttack());
                break;
            case IceBossStates.Ground_Smash:
                Debug.Log("smash");
                StartCoroutine(GroundSmash());
                break;
            case IceBossStates.Stunned:
                StartCoroutine(Stunned());
                break;
            case IceBossStates.Death:
                Debug.Log("death");
                StartCoroutine(DeathRoutine());
                break;
            default:
                break;
        }
    }

    private IEnumerator FindPlayer() {
        while (true) {
            isMoving = true;
            // Calculate the direction towards the player, ignoring the Y-axis
            Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);

            // Rotate to face the player without changing the Y-axis
            transform.DOLookAt(targetPosition, 0.2f);

            // Calculate the direction towards the player
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Move towards the player
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

            // Check the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distanceToPlayer <= distanceThresholdToPlayer)
            {
                currentState = IceBossStates.Ice_Breath;
                SetState(currentState);
                isMoving = false;
                yield break; // Exit the coroutine
            }

            yield return null;
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Pillar")) {
            animator.SetTrigger("Charge_Hurt");
            currentState = IceBossStates.Stunned;
            StopAllCoroutines();
            SetState(currentState);
        }
        else if (collision.gameObject.CompareTag("BossWall")) {
            currentState = IceBossStates.Find_Player;
            StopAllCoroutines();
            SetState(currentState);
        }
    }


    private IEnumerator Idle() {
        // Implement Idle logic
        yield return null;
    }

    private IEnumerator IceBreath() {
        animator.SetTrigger("Ice_Breath");
        yield return new WaitForSeconds(4);
        currentState = IceBossStates.Swipe_Attack;
        SetState(currentState);
        yield return null;
    }

    private IEnumerator Charge() {
        Debug.Log("Charge");
        Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;
        yield return new WaitForSeconds(3);
        isCharging = true;
        animator.SetBool("isCharging", isCharging);
        animator.SetTrigger("Charge");

        transform.DOLookAt(targetPosition, 0.5f);

        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration) {
            rb.MovePosition(rb.position + direction * chargeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isCharging", false);
        isCharging = false;
        rb.velocity = Vector3.zero;
        SetState(IceBossStates.Find_Player);
    }

    private IEnumerator SwipeAttack() {
        
        for (int iq = 0; iq < 3; iq++) {
            Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);
            animator.SetTrigger("Swipe");
            transform.DOLookAt(targetPosition, 0.2f);
            yield return new WaitForSeconds(2f);
        }
        if(currentPhase == IceBossPhases.Phase_One) {
            currentState = IceBossStates.Find_Player;
            SetState(currentState);
        }
        else {
            currentState = IceBossStates.Ground_Smash;
            SetState(currentState);
        }
        yield return null;
    }

    private IEnumerator GroundSmash() {
        while (true) {
            isMoving = true;
            // Calculate the direction towards the player, ignoring the Y-axis
            Vector3 targetPosition = new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z);

            // Rotate to face the player without changing the Y-axis
            transform.DOLookAt(targetPosition, 0.2f);

            // Calculate the direction towards the player
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Move towards the player
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

            // Check the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            if (distanceToPlayer <= distanceThresholdToPlayer) {
                animator.SetTrigger("Smash");
                rb.AddForce(0, 5000, 0, ForceMode.Impulse);
                SetState(IceBossStates.Charge);
                isMoving = false;
                yield break; // Exit the coroutine
            }

            yield return null;
        }
    }
    private IEnumerator Stunned() {
        yield return new WaitForSeconds(4);
        currentState = IceBossStates.Find_Player;
        SetState(currentState);
    }

    private IEnumerator DeathRoutine() {
        // Implement Death logic
        yield return null;
    }

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        // Handle additional damage logic
    }

    protected override void Move() {
        throw new System.NotImplementedException();
    }

    protected override void Attack() {
        throw new System.NotImplementedException();
    }

    protected override void Death() {
        base.Death();
        SetState(IceBossStates.Death);
    }
}

public enum IceBossStates
{
    Idle,
    Ice_Breath,
    Find_Player,
    Charge,
    Swipe_Attack,
    Ground_Smash,
    Stunned,
    Death
}

public enum IceBossPhases
{
    Waiting,
    Phase_One,
    Phase_Two,
    Phase_Three
}
