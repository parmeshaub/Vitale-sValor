using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private CastableScript castManager;
    private MagicMoveSO activeCastableMagic;
    private int activeNum;
    private PlayerCombat playerCombat;

    [SerializeField] private Image slot1Image;
    [SerializeField] private Image slot2Image;
    [SerializeField] private Image slot3Image;
    [SerializeField] private Image slot4Image;

    private bool magic1OnCooldown = false;
    private bool magic2OnCooldown = false;
    private bool magic3OnCooldown = false;
    private bool magic4OnCooldown = false;

    private void Awake() {
        instance = this;
        playerInputManager = PlayerInputManager.instance;
        playerInput = playerInputManager.playerInput;

        playerInput.Gameplay.Magic1.started += ActivateMagic1;
        playerInput.Gameplay.Magic2.started += ActivateMagic2;
        playerInput.Gameplay.Magic3.started += ActivateMagic3;
        playerInput.Gameplay.Magic4.started += ActivateMagic4;
        playerInput.Gameplay.LightAttack.started += CheckCast;

    }

    private void Start() {
        UpdateUI();
        playerCombat = PlayerCombat.Instance;
    }

    private void ActivateMagic1(InputAction.CallbackContext context) {
        if (magic1OnCooldown) return;
        if (magicMoves[0].name == "Null") return;
        CheckEnumType(0);
    }
    private void ActivateMagic2(InputAction.CallbackContext context) {
        if (magic2OnCooldown) return;
        if (magicMoves[1].name == "Null") return;
        CheckEnumType(1);
    }
    private void ActivateMagic3(InputAction.CallbackContext context) {
        if (magic3OnCooldown) return;
        if (magicMoves[2].name == "Null") return;
        CheckEnumType(2);
    }
    private void ActivateMagic4(InputAction.CallbackContext context) {
        if (magic4OnCooldown) return;
        if (magicMoves[3].name == "Null") return;
        CheckEnumType(3);
    }

    private void CheckEnumType(int magicNum) {
        //Debug.Log("Checking Enum");

        if (magicMoves[magicNum].typeOfSkill == TypeOfSkill.INSTANT) {
            if (magicMoves[magicNum].name == "Ablaze" || magicMoves[magicNum].name == "Glaciate")
            {
                if (playerCombat.isEnchanted) return;
                magicMoves[magicNum].Activate();
                StartCoroutine(StartMagicCoolDown(magicNum));
            }
            magicMoves[magicNum].Activate();
            StartCoroutine(StartMagicCoolDown(magicNum));
        }
        else if (castManager.isCasting) {
            castManager.TurnOffCast();
            activeCastableMagic = null;
            activeNum = 0;
        }
        else if (magicMoves[magicNum].typeOfSkill == TypeOfSkill.CASTABLE) {
            castManager.TurnOnCast(magicMoves[magicNum]);
            castManager.numHolder = magicNum;
            activeCastableMagic = magicMoves[magicNum];
            activeNum = magicNum;
        }
    }

    private IEnumerator StartMagicCoolDown(int magicNum) {
        //Debug.Log("Rannnnkn");
        switch (magicNum) {
            case 0:
                magic1OnCooldown = true;
                slot1Image.color = Color.gray;
                yield return new WaitForSeconds(magicMoves[magicNum].coolDownTiming);
                magic1OnCooldown = false;
                slot1Image.color = Color.white;
                break;

            case 1:
                magic2OnCooldown = true;
                slot2Image.color = Color.gray;
                yield return new WaitForSeconds(magicMoves[magicNum].coolDownTiming);
                magic2OnCooldown = false;
                slot2Image.color = Color.white;
                break;

            case 2:
                magic3OnCooldown = true;
                slot3Image.color = Color.gray;
                yield return new WaitForSeconds(magicMoves[magicNum].coolDownTiming);
                magic3OnCooldown = false;
                slot3Image.color = Color.white;
                break;

            case 3:
                magic4OnCooldown = true;
                slot4Image.color = Color.gray;
                yield return new WaitForSeconds(magicMoves[magicNum].coolDownTiming);
                magic4OnCooldown = false;
                slot4Image.color = Color.white;
                break;

            default:
                break;
        }
    }

    private void CheckCast(InputAction.CallbackContext context) {
        if (context.started) {
            if(activeCastableMagic != null && castManager.isCasting) {
                activeCastableMagic.Activate();
                castManager.TurnOffCast();
                StartCoroutine(StartMagicCoolDown(activeNum));
                activeCastableMagic = null;
                activeNum = 0;
            }
        }
    }

    public void UpdateUI() {
        slot1Image.sprite = magicMoves[0].icon;
        slot2Image.sprite = magicMoves[1].icon;
        slot3Image.sprite = magicMoves[2].icon;
        slot4Image.sprite = magicMoves[3].icon;
    }
}
