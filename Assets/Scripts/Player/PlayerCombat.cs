using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using SHG.AnimatorCoder;

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
    private bool isInCombatMode = false;
    [SerializeField] private float timeToTarget = 0.3f;
    [SerializeField] private float timeToLookTarget = 0.1f;
    [SerializeField] private float stoppingDistance = 1.5f; //Stopping Distance from player to target when moving`
    [SerializeField] private float detectionRadius = 6.0f; // How close the player can be to enemy before moving.

    //Unity Events
    public UnityEvent attackStart;
    public UnityEvent attackEnd;

    //public delegate void playerAttackStartEvent();
    //public static event playerAttackStartEvent OnPlayerAttackStartEvent;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        battleSphereDetection = GetComponentInChildren<BattleSphereDetection>();

        playerInput = playerInputManager.playerInput;
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        playerInput.Gameplay.LightAttack.started += PerformLightAttack;
        playerInput.Gameplay.LightAttack.canceled += PerformLightAttack;
        playerInput.Gameplay.HeavyAttack.started += PerformHeavyAttack;

    }

    private void Start()
    {
        if(attackStart == null) attackStart = new UnityEvent();
        if(attackEnd == null) attackEnd = new UnityEvent();
    }

    private void PerformLightAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (CheckDistanceFromEnemy())
            {
                LookAtTarget();
            }
            else
            {
                MoveToTarget();
            }
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

        if(enemyTransform == null)
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
            if(distance >= detectionRadius)
            {
                //enemy is far
                return false;
            }
        }
        //enemy is close
        return true;
    }
}
