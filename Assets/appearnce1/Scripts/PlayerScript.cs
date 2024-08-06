using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float skateSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float dodgeDuration = 0.2f; // Duration of the dodge
    [SerializeField] private float jumpPower;
    [SerializeField] private float airMultiplier;
    public Transform orientation;

    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    bool isSprint = false;
    bool isIceSkating = false; // Ice skating state

    public float groundDrag;
    public float iceDrag; // Reduced drag on ice

    public float playerHeight;
    public LayerMask layerMask;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Animator animator;

    [SerializeField] Transform dodgeLocation;

    private void Start()
    {
        moveSpeed = walkSpeed;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, layerMask);
        MyInput();

        // Apply appropriate drag
        if (grounded || OnSlope())
        {
            rb.drag = isIceSkating ? iceDrag : groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        Vector3 playerVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float playerMoveSpeed = playerVel.magnitude;
        animator.SetFloat("moveSpeed", playerMoveSpeed);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleIceSkating();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            // Apply extra force to stick the player to the slope
            rb.AddForce(Vector3.down * 20f, ForceMode.Force);
        }
        else
        {
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        rb.useGravity = true;
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Dodge()
    {
        animator.SetTrigger("dodge");
        rb.velocity = new Vector3(0, 0, 0);
        StartCoroutine(SmoothDodge());
    }

    private IEnumerator SmoothDodge()
    {
        float timeElapsed = 0;
        Vector3 dodgeDirection = (dodgeLocation.position - transform.position).normalized;

        // Apply an initial impulse force for immediate movement
        rb.AddForce(dodgeDirection * dodgeForce, ForceMode.Impulse);

        while (timeElapsed < dodgeDuration)
        {
            // Gradually adjust the velocity to smooth out the movement
            rb.velocity = Vector3.Lerp(rb.velocity, dodgeDirection * dodgeForce, timeElapsed / dodgeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final velocity is set to zero to stop the dodge
        rb.velocity = Vector3.zero;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.3f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void OnSprint()
    {
        isSprint = !isSprint;
        if (isSprint)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    void OnDodge()
    {
        Debug.Log("dodge");
        Dodge();
    }

    void OnJump()
    {
        if ( isIceSkating == false)
        {
            if (grounded || OnSlope())
            {
                exitingSlope = true;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);

                Invoke(nameof(ResetJump), 0.2f);
            }
        }
        
    }

    private void ResetJump()
    {
        exitingSlope = false;
    }

    private void ToggleIceSkating()
    {
        isIceSkating = !isIceSkating;
        if (isIceSkating)
        {
            Debug.Log("Ice Skating Mode Enabled");
            moveSpeed = skateSpeed; // Increase speed for ice skating
            animator.SetBool("skating", true);
        }
        else
        {
            Debug.Log("Ice Skating Mode Disabled");
            moveSpeed = walkSpeed; // Reset speed when not ice skating
            animator.SetBool("skating", false);
        }
    }
}
