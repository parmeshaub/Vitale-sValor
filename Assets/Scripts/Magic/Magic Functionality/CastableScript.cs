using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableScript : MonoBehaviour
{
    [SerializeField] private GameObject castableDecal;
    public bool isCasting;
    private MagicMoveSO magicToCast;
    private PlayerCombat playerCombat;
    private CameraManager cameraManager;

    private void Awake() {
        playerCombat = PlayerCombat.Instance;
        cameraManager = CameraManager.instance;
    }

    private void Start() {
        castableDecal.SetActive(false);
    }

    public void TurnOnCast(MagicMoveSO magicToCast) {
        castableDecal.gameObject.SetActive(true);
        isCasting = true;
        cameraManager.SwitchToMagicCamera();
    }

    public void TurnOffCast() {
        castableDecal.gameObject.SetActive(false);
        isCasting = false;
        cameraManager.SwitchToMagicCamera();
    }

    public void ActivateMagicMove() {
        if(isCasting) {
            magicToCast.Activate();
        }
        else {
            return;
        }
        isCasting = false;
        TurnOffCast();
    }
}
