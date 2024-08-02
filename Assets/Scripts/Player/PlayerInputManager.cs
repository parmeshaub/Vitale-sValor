using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerInput playerInput;

    private void Awake()
    {
        instance = this;
        playerInput = new PlayerInput();
    }

    public void SwitchToGameplayActionMap()
    {
        playerInput.UI.Disable();
        playerInput.Dialogue.Disable();
        playerInput.Gameplay.Enable();
    }

    public void SwitchToUIActionMap()
    {
        playerInput.UI.Enable();
        playerInput.Dialogue.Disable();
        playerInput.Gameplay.Disable();
    }

    public void SwitchToDialogueActionMap()
    {
        playerInput.Dialogue.Enable();
        playerInput.UI.Disable();
        playerInput.Gameplay.Disable();
    }
    
}
