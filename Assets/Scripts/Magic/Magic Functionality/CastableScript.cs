using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableScript : MonoBehaviour
{
    [SerializeField] private GameObject castableDecal;
    public bool isCasting;
    private MagicMoveSO magicToCast;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MagicManager magicManager;
    public int numHolder;

    private void Start() {
        castableDecal.SetActive(false);
    }

    public void TurnOnCast(MagicMoveSO magicToCast) {
        castableDecal.gameObject.SetActive(true);
        isCasting = true;
        cameraManager.TurnOnMagicCamera();
    }

    public void TurnOffCast() {
        castableDecal.gameObject.SetActive(false);
        isCasting = false;
        cameraManager.TurnOffMagicCamera();
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
