using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerMagicManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    private Animator animator;

    [SerializeField] private VisualEffect volley;
    [SerializeField] private VisualEffect judgement;
    [SerializeField] private VisualEffect razor;
   

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInput = playerInputManager.playerInput;
        animator = GetComponentInChildren<Animator>();

        playerInput.Gameplay.Magic1.started += PerformMagicSlot1;
        playerInput.Gameplay.Magic2.started += PerformMagicSlot2;
        playerInput.Gameplay.Magic3.started += PerformMagicSlot3;
        playerInput.Gameplay.Magic4.started += PerformMagicSlot4;
    }

    private void Start()
    {
        volley.Stop();
        judgement.Stop();
        razor.Stop();
    }

    private void PerformMagicSlot1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            volley.Play();
            animator.SetTrigger("Volley");
        }
    }

    private void PerformMagicSlot2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            judgement.Play();
            animator.SetTrigger("Judgement");
        }
    }

    private void PerformMagicSlot3(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            razor.Play();
            animator.SetTrigger("RazorFangs");
        }
    }

    private void PerformMagicSlot4(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }
}
