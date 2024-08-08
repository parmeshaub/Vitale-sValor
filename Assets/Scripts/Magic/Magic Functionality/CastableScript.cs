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

    private void Start() {
        castableDecal.SetActive(false);
    }

    public void TurnOnCast(MagicMoveSO magicToCast) {
        castableDecal.gameObject.SetActive(true);
        isCasting = true;
        cameraManager.TurnOnMagicCamera();
    }

    public void TurnOffCast() {
        Debug.Log("cast off2");
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
