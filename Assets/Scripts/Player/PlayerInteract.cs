using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    public GameObject currentInteractable;
    private InteractSphere interactSphere;
    private void Awake()
    {
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;
        interactSphere = GetComponent<InteractSphere>();

        playerInput.Gameplay.Interact.started += Interact;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (currentInteractable == null) return;

        if (context.started )
        {
            Interactable interactable = currentInteractable.GetComponent<Interactable>();
            interactable.Interact();
        }
    }
}
