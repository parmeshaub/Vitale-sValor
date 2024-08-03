using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerInput playerInput;
    private CameraManager cameraManager;

    private void Awake()
    {
        instance = this;
        cameraManager = CameraManager.instance;
        playerInput = new PlayerInput();
    }

    public void SwitchToGameplayActionMap()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput.UI.Disable();
        playerInput.Dialogue.Disable();
        playerInput.Gameplay.Enable();

        cameraManager.UnfreezeCamera();
    }

    public void SwitchToUIActionMap()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerInput.UI.Enable();
        playerInput.Dialogue.Disable();
        playerInput.Gameplay.Disable();

        cameraManager.FreezeCamera();
    }

    public void SwitchToDialogueActionMap()
    {
        playerInput.Dialogue.Enable();
        playerInput.UI.Disable();
        playerInput.Gameplay.Disable();

        cameraManager.FreezeCamera();
    }
    
}
