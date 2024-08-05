using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicManager : MonoBehaviour
{
    public static MagicManager instance;
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    public MagicMoveSO[] magicMoves = new MagicMoveSO[4];
    public MagicMoveSO nullMagic;

    private void Awake() {
        instance = this;
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;

        playerInput.Gameplay.Magic1.started += ActivateMagic1;
        playerInput.Gameplay.Magic2.started += ActivateMagic2;
        playerInput.Gameplay.Magic3.started += ActivateMagic3;
        playerInput.Gameplay.Magic4.started += ActivateMagic4;

    }

    private void ActivateMagic1(InputAction.CallbackContext context) {
        if (magicMoves[0].name == "Null") {
            Debug.Log("Cant do nth"); 
            return;
        }
        else {
            magicMoves[0].Activate();
        }
    }
    private void ActivateMagic2(InputAction.CallbackContext context) {
        if (magicMoves[1].name == "Null") {
            Debug.Log("Cant do nth");
            return;
        }
        else {
            magicMoves[1].Activate();
        }
    }
    private void ActivateMagic3(InputAction.CallbackContext context) {
        if (magicMoves[2].name == "Null") {
            Debug.Log("Cant do nth");
            return;
        }
        else {
            magicMoves[2].Activate();
        }
    }
    private void ActivateMagic4(InputAction.CallbackContext context) {
        if (magicMoves[3].name == "Null") {
            Debug.Log("Cant do nth");
            return;
        }
        else {
            magicMoves[3].Activate();
        }
    }

    private void UpdateUI() {

    }
}
