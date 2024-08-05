using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
    public static MagicManager instance;
    private PlayerInputManager playerInputManager;
    private PlayerInput playerInput;
    public MagicMoveSO[] magicMoves = new MagicMoveSO[4];
    public MagicMoveSO nullMagic;

    [SerializeField] private Image slot1Image;
    [SerializeField] private Image slot2Image;
    [SerializeField] private Image slot3Image;
    [SerializeField] private Image slot4Image;

    private void Awake() {
        instance = this;
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;

        playerInput.Gameplay.Magic1.started += ActivateMagic1;
        playerInput.Gameplay.Magic2.started += ActivateMagic2;
        playerInput.Gameplay.Magic3.started += ActivateMagic3;
        playerInput.Gameplay.Magic4.started += ActivateMagic4;

    }

    private void Start() {
        UpdateUI();
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

    public void UpdateUI() {
        slot1Image.sprite = magicMoves[0].icon;
        slot2Image.sprite = magicMoves[1].icon;
        slot3Image.sprite = magicMoves[2].icon;
        slot4Image.sprite = magicMoves[3].icon;
    }
}
