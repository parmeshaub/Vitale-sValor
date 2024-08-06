using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    [HideInInspector] public GameObject currentInteractable;
    private InteractSphere interactSphere;
    private void Awake() {
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;
        interactSphere = GetComponentInChildren<InteractSphere>();

        playerInput.Gameplay.Interact.started += Interact;
    }

    private void Interact(InputAction.CallbackContext context) {
        if (currentInteractable == null) Debug.Log("currentInteractable is null");

        if (context.started) {
            Interactable interactable = currentInteractable.GetComponent<Interactable>();
            if (interactable != null) {
                interactable.Interact();
            }
            else {
                Debug.LogError("Interactable NULL");
            }
            
        }
    }
}