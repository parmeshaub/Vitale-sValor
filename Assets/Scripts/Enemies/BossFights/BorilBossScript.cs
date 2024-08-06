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
    private float distanceThresholdToPlayer = 2;

    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;
    private int swipeCount = 0;

    private static readonly int isMovingHash = Animator.StringToHash("isMoving");


    private void Awake() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
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
        currentPhase = IceBossPhases.Phase_One;
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


    private IEnumerator Idle() {
        // Implement Idle logic
        yield return null;
    }

    private IEnumerator IceBreath() {
        //animator.SetTrigger();
        currentState = IceBossStates.Swipe_Attack;
        SetState(currentState);
        yield return null;
    }

    private IEnumerator Charge() {
        yield return null;
    }

    private IEnumerator SwipeAttack() {
        // Implement Swipe Attack logic
        yield return null;
    }

    private IEnumerator GroundSmash() {
        // Implement Ground Smash logic
        yield return null;
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
    Death
}

public enum IceBossPhases
{
    Waiting,
    Phase_One,
    Phase_Two,
    Phase_Three
}
