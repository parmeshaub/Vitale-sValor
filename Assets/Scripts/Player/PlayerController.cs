using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    private PlayerInput playerInput;
    public CharacterController characterController;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentSprintMovement;
    private Vector3 appliedMovement;
    private Vector3 cameraRelativeMovement;

    public bool isMovementPressed;
    public bool isSprintPressed;
    [SerializeField] private float rotationFactorPerFrame = 15.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;
    [SerializeField] private float moveSpeed = 4.0f;

    // Gravity
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float groundedGravity = -.05f;

    // Jump
    bool isJumpPressed = false;
    private float initialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 1.0f;
    [SerializeField] private float maxJumpTime = 0.75f;
    private bool isJumping = false;
    private bool isJumpAnimating = false;

    // Dash
    private bool isDashing = false;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashMultiplier = 6f;
    [SerializeField] private float dashCoolDown = 1f;
    private bool isDashOnCoolDown = false;

    // Combat
    private PlayerCombat playerCombat;
    private bool isAttacking = false;

    // For Animation
    private Animator animator;
    private float velocityXZ;

    // Animation Hashes
    private readonly static int velocityXZHash = Animator.StringToHash("VelocityXZ");
    private readonly static int isMovementHeldHash = Animator.StringToHash("isMovementHeld");
    private readonly static int isGroundedHash = Animator.StringToHash("isGrounded");
    private readonly static int jumpHash = Animator.StringToHash("Jump");
    private readonly static int dodgeHash = Animator.StringToHash("Dodge_Front");
    private readonly static int sprintHash = Animator.StringToHash("isSprinting");
    private readonly static int blockHash = Animator.StringToHash("isBlocking");

    private Coroutine exitCoroutine;
    private bool isWaitingExit = false;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerCombat = GetComponent<PlayerCombat>();

        playerInput = playerInputManager.playerInput;
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        playerInput.Gameplay.Move.started += OnMovementInput;
        playerInput.Gameplay.Move.canceled += OnMovementInput;
        playerInput.Gameplay.Move.performed += OnMovementInput;
        playerInput.Gameplay.Sprint.started += OnRun;
        playerInput.Gameplay.Sprint.canceled += OnRun;
        playerInput.Gameplay.Jump.started += OnJump;
        playerInput.Gameplay.Jump.canceled += OnJump;
        playerInput.Gameplay.Dash.started += OnDash;
        playerInput.Gameplay.Dash.canceled += OnDash;


        SetupJumpVariables();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInputManager.SwitchToGameplayActionMap();
    }

    void Update()
    {
        HandleAnimation();
        if (!isAttacking)
        {
            if (!playerCombat.isBlocking)
            {
                HandleRotation();
                HandleMovement();
            }

            HandleGravity();

            if (!playerCombat.isBlocking)
            {
                HandleJump();
            }

        }
    }

    private void OnEnable()
    {
        playerInput.Gameplay.Enable();
        playerCombat.attackStart.AddListener(CombatModeEnter);
        playerCombat.attackEnd.AddListener(CombatModeExit);
    }

    private void OnDisable()
    {
        playerInput?.Gameplay.Disable();
        playerCombat.attackStart.RemoveListener(CombatModeEnter);
        playerCombat.attackEnd.RemoveListener(CombatModeExit);
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * moveSpeed;
        currentMovement.z = currentMovementInput.y * moveSpeed;
        currentSprintMovement.x = currentMovement.x * sprintMultiplier;
        currentSprintMovement.z = currentMovement.z * sprintMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void HandleJump()
    {
        if (isJumpPressed && characterController.isGrounded)
        {
            animator.SetTrigger(jumpHash);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    private void HandleAnimation()
    {
        velocityXZ = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
        animator.SetFloat(velocityXZHash, velocityXZ);
        animator.SetBool(isMovementHeldHash, isMovementPressed);
        animator.SetBool(isGroundedHash, characterController.isGrounded);
        animator.SetBool(sprintHash, isSprintPressed);
        animator.SetBool(blockHash, playerCombat.isBlocking);
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        if (isDashing)
        {
            positionToLookAt.x = appliedMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = appliedMovement.z;
        }
        else
        {
            positionToLookAt.x = cameraRelativeMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = cameraRelativeMovement.z;
        }

        if (positionToLookAt != Vector3.zero)
        {
            Quaternion currentRotation = transform.rotation;

            if (isMovementPressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
            }
        }
    }

    private void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2f;

        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                isJumpAnimating = false;
            }
            currentMovement.y = groundedGravity;
            currentSprintMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -20f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
        }
    }

    private void HandleMovement()
    {
        if (isDashing)
        {
            appliedMovement = transform.forward * (moveSpeed * dashMultiplier);
        }
        else if (isSprintPressed)
        {
            appliedMovement.x = currentMovement.x * sprintMultiplier;
            appliedMovement.z = currentMovement.z * sprintMultiplier;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;
        }

        if (isDashing)
        {
            characterController.Move(appliedMovement * Time.deltaTime);
        }
        else
        {
            cameraRelativeMovement = ConvertToCameraSpace(appliedMovement);
            characterController.Move(cameraRelativeMovement * Time.deltaTime);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        animator.SetTrigger(dodgeHash);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        StartCoroutine(DashCoolDown());
    }

    private IEnumerator DashCoolDown()
    {
        isDashOnCoolDown = true;
        yield return new WaitForSeconds(dashCoolDown);
        isDashOnCoolDown = false;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        isSprintPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing)
        {
            if (!isDashOnCoolDown)
            {
                StartCoroutine(Dash());
            }
        }
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    void CombatModeEnter()
    {
        isAttacking = true;
    }

    void CombatModeExit()
    {

        isAttacking = false;
    }
}
