using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Name: Lee Zhi Hui, Shaun
/// Description:
/// This code handles the combat functionality of the player,
/// Does not include getting hurt or death
/// </summary>
public class PlayerCombat : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private BattleSphereDetection battleSphereDetection;

    PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    //Combat
    private bool inCombatMode = false;
    [SerializeField] private float timeToTarget = 0.3f;
    [SerializeField] private float timeToLookTarget = 0.1f;
    [SerializeField] private float stoppingDistance = 1.5f; //Stopping Distance from player to target when moving`
    [SerializeField] private float detectionRadius = 6.0f; // How close the player can be to enemy before moving.
    [SerializeField] private float lightAttackCoolDown = 1f;
    private bool isLightAttackOnCooldown = false;
    private int lightAttackComboStep = 0;

    private Coroutine resetComboCoroutine;

    //Combat Cooldown
    [SerializeField] private float lightAttack1CoolDown;
    [SerializeField] private float lightAttack2CoolDown;
    [SerializeField] private float lightAttack3CoolDown;
    [SerializeField] private float lightAttack4CoolDown;

    //Unity Events
    [HideInInspector] public UnityEvent attackStart;
    [HideInInspector] public UnityEvent attackEnd;

    //public delegate void playerAttackStartEvent();
    //public static event playerAttackStartEvent OnPlayerAttackStartEvent;

    //Animation Hashes
    private readonly static int drawSwordHash = Animator.StringToHash("Draw_Sword");
    private readonly static int keepSwordHash = Animator.StringToHash("Keep_Sword");
    private readonly static int inCombatHash = Animator.StringToHash("InCombat");

    //----COMBAT HASHES-----

    //Light Attack Hashes
    private readonly static int lightAtk1Hash = Animator.StringToHash("Light_Attack_01");
    private readonly static int lightAtk2Hash = Animator.StringToHash("Light_Attack_02");
    private readonly static int lightAtk3Hash = Animator.StringToHash("Light_Attack_03");
    private readonly static int lightAtk4Hash = Animator.StringToHash("Light_Attack_04");
    //Light Attack 02 Hash
    //Heavy Attack Hash

    private void Update()
    {
        Debug.Log(isLightAttackOnCooldown);
    }

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        battleSphereDetection = GetComponentInChildren<BattleSphereDetection>();
        animator = GetComponentInChildren<Animator>();

        playerInput = playerInputManager.playerInput;
        characterController = GetComponent<CharacterController>();

        playerInput.Gameplay.LightAttack.started += PerformLightAttack;
        playerInput.Gameplay.LightAttack.canceled += PerformLightAttack;
        playerInput.Gameplay.HeavyAttack.started += PerformHeavyAttack;
    }

    private void Start()
    {
        if (attackStart == null) attackStart = new UnityEvent();
        if (attackEnd == null) attackEnd = new UnityEvent();
    }

    private void PerformLightAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        CheckCombatMode();
        if (!inCombatMode) return;

        //If Cooldown is off
        if (!isLightAttackOnCooldown)
        {
            if (CheckDistanceFromEnemy())
            {
                LookAtTarget();
            }
            else
            {
                MoveToTarget();
            }

            LightAttackCombo();
            StartCoroutine(LightAttackCooldown());
        }
        else return;
    }

    private void LightAttackCombo()
    {
        switch (lightAttackComboStep)
        {
            //Attack 1
            case 0:
                animator.SetTrigger(lightAtk1Hash);
                lightAttackCoolDown = lightAttack1CoolDown;
                break;

            //Attack 2
            case 1:
                animator.SetTrigger(lightAtk2Hash);
                lightAttackCoolDown = lightAttack2CoolDown;
                break;

            //Attack 3
            case 2:
                animator.SetTrigger(lightAtk3Hash);
                lightAttackCoolDown = lightAttack3CoolDown;
                break;

            case 3:
                animator.SetTrigger(lightAtk4Hash);
                lightAttackCoolDown = lightAttack4CoolDown;
                break;
            default:
                break;
        }

        lightAttackComboStep++;

        //Reset Combo
        if (lightAttackComboStep > 3)
        {
            lightAttackComboStep = 0;
        }
    }

    private void PerformHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CheckDistanceFromEnemy()) LookAtTarget();
            else MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        Transform enemyTransform = battleSphereDetection.FindClosestEnemy();
        if (enemyTransform == null)
        {
            return;
        }
        else
        {
            attackStart.Invoke();

            // Calculate the look at target ignoring the Y axis
            Vector3 lookAtTarget = new Vector3(enemyTransform.position.x, transform.position.y, enemyTransform.position.z);

            transform.DOLookAt(lookAtTarget, timeToLookTarget);

            Vector3 directionToEnemy = (enemyTransform.position - transform.position).normalized;
            Vector3 targetPosition = enemyTransform.position - directionToEnemy * stoppingDistance;
            targetPosition.y = enemyTransform.position.y - 1;

            transform.DOMove(targetPosition, timeToTarget).OnComplete(() =>
            {
                attackEnd.Invoke();
            });
        }
    }

    private void LookAtTarget()
    {
        Transform enemyTransform = battleSphereDetection.FindClosestEnemy();

        if (enemyTransform == null)
        {
            return;
        }

        attackStart.Invoke();

        // Calculate the look at target ignoring the Y axis
        Vector3 lookAtTarget = new Vector3(enemyTransform.position.x, transform.position.y, enemyTransform.position.z);
        transform.DOLookAt(lookAtTarget, timeToLookTarget);
        attackEnd.Invoke();
    }

    private bool CheckDistanceFromEnemy()
    {
        Transform enemyTransform = battleSphereDetection.FindClosestEnemy();

        if (enemyTransform != null)
        {
            float distance = Vector3.Distance(transform.position, enemyTransform.position);
            if (distance >= detectionRadius)
            {
                //enemy is far
                return false;
            }
        }
        //enemy is close
        return true;
    }

    //To be called before every attack.
    private void CheckCombatMode()
    {
        //Turn on Combat mode if False.
        if (!inCombatMode)
        {
            inCombatMode = true;
            animator.SetTrigger(drawSwordHash);
            animator.SetBool(inCombatHash, true);
        }
    }

    private void TurnOffCombatMode()
    {
        inCombatMode = false;
        animator.SetTrigger(keepSwordHash);
        animator.SetBool(inCombatHash, false);
    }

    private IEnumerator LightAttackCooldown()
    {
        isLightAttackOnCooldown = true;
        yield return new WaitForSeconds(lightAttackCoolDown);
        isLightAttackOnCooldown = false;
    }

    private IEnumerator ResetComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lightAttackComboStep = 0;
    }
}
