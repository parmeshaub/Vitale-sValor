using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private CharacterController controller;
    private Animator anim;

    private Vector3 desiredMoveDir;
    private Vector3 moveVector;

    public Vector2 moveAxis;
    private float verticalVel;

    [Header("Settings")]
    private float movementSpeed;
    [SerializeField] private float originalMovementSpeed;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float fallSpeed = 0.2f;
    [SerializeField] private float sprintSpeed;
    public float acceleration = 1;

    [Header("bool")]
    [SerializeField] bool blockRotationPlayer;
    private bool isGrounded;

    private bool isSprint;

    private void Start()
    {
        movementSpeed = originalMovementSpeed;

        //Get the animator, camera and controller
        anim = this.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        InputMagnitude();

        isGrounded = controller.isGrounded;

        if (isGrounded)
            verticalVel -= 0;
        else
            verticalVel -= 1;

        moveVector = new Vector3(0, verticalVel * fallSpeed * Time.deltaTime, 0);
        controller.Move(moveVector);
    }

    void PlayerMoveAndRotation()
    {
        //var cam = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDir = forward * moveAxis.y + right * moveAxis.x;

        if (blockRotationPlayer == false)
        {
            //Camera
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDir), rotationSpeed * acceleration);
            controller.Move(desiredMoveDir * Time.deltaTime * (movementSpeed * acceleration));
        }
        else
        {
            //Strafe
            controller.Move((transform.forward * moveAxis.y + transform.right * moveAxis.y) * Time.deltaTime * (movementSpeed * acceleration));
        }
    }

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), rotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {
        var forward = cam.transform.forward;

        desiredMoveDir = forward;
        Quaternion lookAtRotation = Quaternion.LookRotation(desiredMoveDir);
        Quaternion lookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, lookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        t.rotation = Quaternion.Slerp(transform.rotation, lookAtRotationOnly_Y, rotationSpeed);
    }

    void InputMagnitude()
    {
        //Calculate the Input Magnitude
        float inputMagnitude = new Vector2(moveAxis.x, moveAxis.y).sqrMagnitude;

        //Physically move player
        if (inputMagnitude > 0.1f)
        {
            anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, .1f, Time.deltaTime);
            PlayerMoveAndRotation();
        }
        else
        {
            anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, .1f, Time.deltaTime);
        }
    }

    #region Input

    public void OnMove(InputValue value)
    {
        moveAxis.x = value.Get<Vector2>().x;
        moveAxis.y = value.Get<Vector2>().y;
    }

    public void OnSprint()
    {
        if (isSprint == false)
        {
            isSprint = true;
            movementSpeed = sprintSpeed;
        }
        else
        {
            isSprint = false;
            movementSpeed = originalMovementSpeed;
        }
        
    }

    #endregion

    private void OnDisable()
    {
        anim.SetFloat("InputMagnitude", 0);
    }

}
