using DG.Tweening;
using System.Collections;
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
    public static PlayerCombat Instance;

    private PlayerInputManager playerInputManager;
    public BattleSphereDetection battleSphereDetection;
    private SwordManager swordManager;
    private PlayerController playerController;

    private PlayerInput playerInput;
    public CharacterController characterController;
    public Animator animator;
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private CastableScript castManager;

    //Combat Editables
    [SerializeField] private float timeToTarget = 0.3f;
    [SerializeField] private float timeToLookTarget = 0.1f;
    [SerializeField] private float stoppingDistance = 1.5f; //Stopping Distance from player to target when moving`
    [SerializeField] private float detectionRadius = 6.0f; // How close the player can be to enemy before moving.

    //Combat Variables
    private bool inCombatMode = false;
    private float lightAttackCoolDown = 1f;
    private float heavyAttackCoolDown = 1f;
    private bool isLightAttackOnCooldown = false;
    private bool isHeavyAttackOnCooldown = false;
    private bool isParryOnCoolDown = false;
    public bool isBlocking = false;
    private int lightAttackComboStep = 0;
    private int heavyAttackComboStep = 0;
    private Coroutine resetLightAttackComboCoroutine;
    private Coroutine resetHeavyAttackComboCoroutine;
    public bool isAttacking = false;

    //Combat Cooldown
    [SerializeField] private float lightAttack1CoolDown;
    [SerializeField] private float lightAttack2CoolDown;
    [SerializeField] private float lightAttack3CoolDown;
    [SerializeField] private float lightAttack4CoolDown;
    [SerializeField] private float heavyAttack1CoolDown;
    [SerializeField] private float heavyAttack2CoolDown;
    [SerializeField] private float heavyAttack3CoolDown;
    [SerializeField] private float heavyAttack4CoolDown;
    [SerializeField] private float parryCoolDown = 1f;

    //Combat to pass.
    public float attackDamage;

    //Unity Events
    [HideInInspector] public UnityEvent attackStart;
    [HideInInspector] public UnityEvent attackEnd;

    #region Variables // Animation Hashes
    //Animation Hashes
    private readonly static int drawSwordHash = Animator.StringToHash("Draw_Sword");
    private readonly static int keepSwordHash = Animator.StringToHash("Keep_Sword");
    private readonly static int inCombatHash = Animator.StringToHash("InCombat");
    private readonly static int parryHash = Animator.StringToHash("Parry");
    private readonly static int blockStartHash = Animator.StringToHash("Block_Start");

    //----COMBAT HASHES-----

    //Light Attack Hashes
    private readonly static int lightAtk1Hash = Animator.StringToHash("Light_Attack_01");
    private readonly static int lightAtk2Hash = Animator.StringToHash("Light_Attack_02");
    private readonly static int lightAtk3Hash = Animator.StringToHash("Light_Attack_03");
    private readonly static int lightAtk4Hash = Animator.StringToHash("Light_Attack_04");
    //Light Attack 02 Hash
    //Heavy Attack Hash
    private readonly static int heavyAtk1Hash = Animator.StringToHash("Heavy_Attack_01");
    private readonly static int heavyAtk2Hash = Animator.StringToHash("Heavy_Attack_02");
    private readonly static int heavyAtk3Hash = Animator.StringToHash("Heavy_Attack_03");
    private readonly static int heavyAtk4Hash = Animator.StringToHash("Heavy_Attack_04");

    #endregion
    private void Update()
    {
    }

    private void Awake()
    {
        Instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
        battleSphereDetection = GetComponentInChildren<BattleSphereDetection>();
        animator = GetComponentInChildren<Animator>();
        swordManager = SwordManager.instance;
        playerController = GetComponent<PlayerController>();

        playerInput = playerInputManager.playerInput;
        characterController = GetComponent<CharacterController>();

        playerInput.Gameplay.LightAttack.performed += PerformLightAttack;
        playerInput.Gameplay.HeavyAttack.performed += PerformHeavyAttack;
        playerInput.Gameplay.Unsheath.performed += TurnOffCombatMode;
        playerInput.Gameplay.Block.started += PerformBlock;
        playerInput.Gameplay.Block.canceled += TurnOffBlock;
    }

    private void Start()
    {
        if (attackStart == null) attackStart = new UnityEvent();
        if (attackEnd == null) attackEnd = new UnityEvent();
    }
    #region Attacks
    private void PerformLightAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        //Perform Parry if blocking
        if (isBlocking)
        {
            if (!isParryOnCoolDown)
            {
                PerformParry();
                return;
            }
        }
        else
        {
            if (!playerController.characterController.isGrounded) return;
            CheckCombatMode();
            if (!inCombatMode) return;
            if (castManager.isCasting) return;

            //If Cooldown is off
            if (!isLightAttackOnCooldown && !isHeavyAttackOnCooldown)
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
    }

    private void LightAttackCombo()
    {
        //Check if theres any previous Coroutine (Combo Reset), Cancel it.
        if(resetLightAttackComboCoroutine != null) StopCoroutine(resetLightAttackComboCoroutine);

        //Perform Combo according to Animation
        switch (lightAttackComboStep)
        {
            //Attack 1
            case 0:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(lightAtk1Hash);
                lightAttackCoolDown = lightAttack1CoolDown;
                break;

            //Attack 2
            case 1:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(lightAtk2Hash);
                lightAttackCoolDown = lightAttack2CoolDown;
                break;

            //Attack 3
            case 2:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(lightAtk3Hash);
                lightAttackCoolDown = lightAttack3CoolDown;
                break;

            case 3:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(lightAtk4Hash);
                lightAttackCoolDown = lightAttack4CoolDown;
                break;
            default:
                break;
        }
        
        //Add Combo Step
        lightAttackComboStep++;

        //Reset Combo
        if (lightAttackComboStep > 3)
        {
            lightAttackComboStep = 0;
        }

        //Start Timer to reset Combo to zero automatically.
        resetLightAttackComboCoroutine = StartCoroutine(ResetLightAttackComboAfterDelay());
    }

    private void PerformHeavyAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isBlocking) return;
        if (!playerController.characterController.isGrounded) return;
        if (!inCombatMode) return;
        if (castManager.isCasting) return;

        CheckCombatMode();

        //If Cooldown is off
        if (!isHeavyAttackOnCooldown && !isLightAttackOnCooldown)
        {
            if (CheckDistanceFromEnemy())
            {
                LookAtTarget();
            }
            else
            {
                MoveToTarget();
            }

            HeavyAttackCombo();
            StartCoroutine(HeavyAttackCoolDown());
        }
        else return;
    }

    private void HeavyAttackCombo()
    {
        //Check if theres any previous Coroutine (Combo Reset), Cancel it.
        if (resetHeavyAttackComboCoroutine != null) StopCoroutine(resetHeavyAttackComboCoroutine);

        //Perform Combo according to Animation
        switch (heavyAttackComboStep)
        {
            //Attack 1
            case 0:
                MoveWhileAttack(2f,0.4f);
                animator.SetTrigger(heavyAtk1Hash);
                heavyAttackCoolDown = heavyAttack1CoolDown;
                break;

            //Attack 2
            case 1:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(heavyAtk2Hash);
                heavyAttackCoolDown = heavyAttack2CoolDown;
                break;

            //Attack 3
            case 2:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(heavyAtk3Hash);
                heavyAttackCoolDown = heavyAttack3CoolDown;
                break;

            case 3:
                MoveWhileAttack(2f, 0.4f);
                animator.SetTrigger(heavyAtk4Hash);
                heavyAttackCoolDown = heavyAttack4CoolDown;
                break;
            default:
                break;
        }

        //Add Combo Step
        heavyAttackComboStep++;

        //Reset Combo
        if (heavyAttackComboStep > 3)
        {
            heavyAttackComboStep = 0;
        }

        //Start Timer to reset Combo to zero automatically.
        resetHeavyAttackComboCoroutine = StartCoroutine(ResetHeavyAttackComboAfterDelay());
    }

    #endregion

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

    public void MoveWhileAttack(float movementAmount, float duration) {
        if (!playerController.characterController.isGrounded) return;
        Transform enemyTransform = battleSphereDetection.FindClosestEnemy();

        // Calculate the initial target position
        Vector3 targetPosition = transform.position + transform.forward * movementAmount;

        // Check if the path to the target position is clear
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, movementAmount)) {
            // If the raycast hits something, adjust the target position to stop before the obstacle
            targetPosition = hit.point - transform.forward * 0.5f; // Adjust this value as needed to stop the player before the obstacle
        }

        if (enemyTransform != null) {
            float distance = Vector3.Distance(transform.position, enemyTransform.position);
            if (distance > 3f) {
                // Move the character using DOTween
                transform.DOMove(targetPosition, duration);
            }
        }
        else {
            // Move the character using DOTween
            transform.DOMove(targetPosition, duration);
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
    private void CheckCombatMode2()
    {
        //Turn on Combat mode if False.
        if (!inCombatMode)
        {
            inCombatMode = true;
            animator.SetBool(inCombatHash, true);
        }
    }

    private void TurnOffCombatMode(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(inCombatMode)
            {
                if (!playerController.characterController.isGrounded) return;
                inCombatMode = false;
                animator.SetTrigger(keepSwordHash);
                animator.SetBool(inCombatHash, false);
            }
        }
    }

    #region Blocking
    private void PerformBlock(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (characterController.isGrounded) {
                CheckCombatMode2();
                LookAtTarget();
                animator.SetTrigger(blockStartHash);
                isBlocking = true;
                isAttacking = false;
            }
        }
    }

    private void TurnOffBlock(InputAction.CallbackContext context)
    {
        if(context.canceled)
        {
            CheckCombatMode2();
            LookAtTarget();
            isBlocking = false;
        }
    }

    private void PerformParry()
    {
        animator.SetTrigger(parryHash);
        StartCoroutine(ParryCoolDown());
    }

    #endregion

    private IEnumerator LightAttackCooldown()
    {
        isLightAttackOnCooldown = true;
        yield return new WaitForSeconds(lightAttackCoolDown);
        isLightAttackOnCooldown = false;
    }

    private IEnumerator HeavyAttackCoolDown()
    {
        isHeavyAttackOnCooldown = true;
        yield return new WaitForSeconds(heavyAttackCoolDown);
        isHeavyAttackOnCooldown = false;
    }

    private IEnumerator ResetLightAttackComboAfterDelay()
    {
        yield return new WaitForSeconds(lightAttackCoolDown + 1f);
        lightAttackComboStep = 0;
    }

    private IEnumerator ResetHeavyAttackComboAfterDelay()
    {
        yield return new WaitForSeconds(heavyAttackCoolDown + 1f);
        heavyAttackComboStep = 0;
    }

    private IEnumerator ParryCoolDown()
    {
        isParryOnCoolDown = true;
        yield return new WaitForSeconds(parryCoolDown);
        isParryOnCoolDown = false;
    }

    public float LightRandomizeDamage()
    {
        if (PlayerStatsManager.instance != null)
        {
            float minDamage = PlayerStatsManager.instance.minLightAttackDamage;
            float maxDamage = PlayerStatsManager.instance.maxLightAttackDamage;

            float attackDamage = Random.Range(minDamage, maxDamage);
            return attackDamage;
        }
        else
        {
            Debug.LogError("PlayerStatsManager instance is not assigned!");
            return 0f;
        }

    }

    public float HeavyRandomizeDamage()
    {
        if (PlayerStatsManager.instance != null)
        {
            float minDamage = PlayerStatsManager.instance.minHeavyAttackDamage;
            float maxDamage = PlayerStatsManager.instance.maxHeavyAttackDamage;

            float attackDamage = Random.Range(minDamage, maxDamage);
            return attackDamage;
        }
        else
        {
            Debug.LogError("PlayerStatsManager instance is not assigned!");
            return 0f;
        }

    }
    
    public void SetIsAttackingTrue() {
        isAttacking = true;
    }
    public void SetIsAttackingFalse() {
        isAttacking = false;
    }

    public bool GetInCombatBool() => inCombatMode;
}
