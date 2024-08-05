using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuneManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;

        playerInput.Gameplay.ActivateRune.started += OpenRuneMenu;
        playerInput.Gameplay.ActivateRune.canceled += CloseRuneMenu;
    }

    private void OpenRuneMenu(InputAction.CallbackContext context) {
        if (!context.started) return;
        Debug.Log("Rune Open");

    }

    private void CloseRuneMenu(InputAction.CallbackContext context) {
        if(!context.canceled) return;
        Debug.Log("Rune Close");
    }
}
