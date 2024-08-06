using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.AI;

public enum IceBossBossState
{
    Idle,
    Roar,
    Patrol,
    Charge,
    Attack,
    Smash,
    Dead
}

public enum IceBossPhase
{
    Waiting,
    PhaseOne,
    PhaseTwo,
    PhaseThree
}

public class IcebearScript : MonoBehaviour
{
    public static IcebearScript instance;

    public IceBossBossState currentState;
    public IceBossPhase currentPhase;

    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private Transform player;

    [Header("Boss Factors")]
    [SerializeField] private int maxHealth;
    private int currentHealth;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float chargeDuration;

    [SerializeField] private float pillarBreakShakePower;
    [SerializeField] CinemachineImpulseSource screenShake;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float patrolSpeed = 3.5f;
    [SerializeField] private float patrolDuration = 4;
    [SerializeField] private float circleSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5;
    public float detectionRadius = 10f;
    private bool isPatrol;
    public bool isCharging;
    private bool roarOnce= false;

    private float circleAngle;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentPhase = IceBossPhase.Waiting;
        agent.enabled = false;
        StartIceBoss();
    }

    private void OnEnable()
    {
        OutTriggerIceBearScript.OnStopIceBearChargeEdge += StopIceBearOffEdge;
        PillarScript.OnIceBearPillarHit += PillarHit;
    }

    private void OnDisable()
    {
        OutTriggerIceBearScript.OnStopIceBearChargeEdge -= StopIceBearOffEdge;
        PillarScript.OnIceBearPillarHit -= PillarHit;
    }

    private void Update()
    {
        if (isPatrol)
        {
            animator.SetFloat("movement_speed", agent.speed);
        }
    }

    public void StartIceBoss()
    {
        currentPhase = IceBossPhase.PhaseOne;
        if(currentPhase == IceBossPhase.PhaseOne)
        {
            SetState(IceBossBossState.Patrol);
            currentHealth = maxHealth;
        }
        
    }

    private void SetState(IceBossBossState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            StopAllCoroutines();

            switch (newState)
            {
                case IceBossBossState.Patrol:
                    agent.enabled = true;
                    StartCoroutine(PatrolRoutine());
                    break;

                case IceBossBossState.Roar:
                    agent.enabled = false;
                    StartCoroutine(RoarRoutine());
                    break;

                case IceBossBossState.Charge:
                    agent.enabled = false;
                    StartCoroutine(ChargeRoutine());
                    break;

                case IceBossBossState.Attack:
                    agent.enabled = true;
                    StartCoroutine(AttackRoutine());
                    break;

                case IceBossBossState.Smash:
                    agent.enabled = false;
                    StartCoroutine(SmashRoutine());
                    break;

                default:
                    break;
            }
        }
    }

    private void StopIceBearOffEdge()
    {
        animator.SetBool("isCharging", false);
        isCharging = false;
        animator.SetTrigger("idle");

        StopAllCoroutines();
        rb.velocity = Vector3.zero;

        Vector3 backwardForce = -transform.forward * 10f;
        rb.AddForce(backwardForce, ForceMode.Impulse);

        SetState(IceBossBossState.Patrol);
    }

    private void PillarHit()
    {
        StartCoroutine(PillarHitWait());
    }

    private IEnumerator PillarHitWait()
    {
        animator.SetBool("isCharging", false);
        isCharging = false;
        animator.SetTrigger("pillar_hit");

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.constraints = RigidbodyConstraints.FreezeAll;

        yield return new WaitForSeconds(4);

        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        SetState(IceBossBossState.Patrol);
    }

    private IEnumerator PatrolRoutine()
    {
        Debug.Log("Patrol");
        isPatrol = true;
        circleAngle = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < patrolDuration)
        {
            if (Vector3.Distance(transform.position, player.position) > detectionRadius)
            {
                MoveTowardsPlayer();
            }
            else
            {
                CirclePlayer();
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetState(IceBossBossState.Attack);
    }

    private void CirclePlayer()
    {
        circleAngle += circleSpeed * Time.deltaTime;
        if (circleAngle > 360f) circleAngle -= 360f;

        Vector3 offset = new Vector3(Mathf.Sin(circleAngle), 0, Mathf.Cos(circleAngle)) * patrolRadius;
        Vector3 targetPosition = player.position + offset;

        agent.speed = patrolSpeed;
        agent.SetDestination(targetPosition);
    }

    private void MoveTowardsPlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private IEnumerator RoarRoutine()
    {
        Debug.Log("Rawr");
        isPatrol = false;
        transform.DOLookAt(player.position, 0.4f);
        animator.SetTrigger("roar");
        yield return new WaitForSeconds(1.55f);
        screenShake.GenerateImpulse(pillarBreakShakePower);
        yield return new WaitForSeconds(4.45f);
        
        if (roarOnce)
        {
            SetState(IceBossBossState.Charge);
            roarOnce = false;
        }
        else // havent roar
        {
            SetState(IceBossBossState.Smash);
            roarOnce = true;
        }
        roarOnce = true;
    }

    private IEnumerator ChargeRoutine()
    {
        Debug.Log("Charge");
        Vector3 direction = (player.position - transform.position).normalized;

        animator.SetBool("isCharging", true);
        isCharging = true;
        animator.SetTrigger("charge");

        transform.DOLookAt(player.position, 0.5f);

        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration)
        {
            rb.MovePosition(rb.position + direction * chargeSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isCharging", false);
        isCharging = false;
        rb.velocity = Vector3.zero;
        SetState(IceBossBossState.Patrol);
    }

    private IEnumerator AttackRoutine()
    {
        Debug.Log("Attack");
        agent.SetDestination(player.position);

        while (agent.remainingDistance > agent.stoppingDistance)
        {
            transform.DOLookAt(player.position, 0.2f);
            yield return null;
        }

        for (int i = 0; i < 3; i++)
        {
            transform.DOLookAt(player.position, 0.2f); // Ensure the boss looks at the player before each attack
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(2f); // Adjust the delay as per the attack animation duration
        }

        SetState(IceBossBossState.Roar);
    }

    private IEnumerator SmashRoutine()
    {
        animator.SetTrigger("Smash");
        rb.AddForce(0, 8, 0, ForceMode.Impulse);
        yield return new WaitForSeconds(4f);

        // Transition to the next state
        SetState(IceBossBossState.Roar);
    }



}
