using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUnlockInteractable : Interactable
{
    [SerializeField] private MagicMoveSO skillToUnlock;
    private MagicLockManager lockManager;

    private void Start() {
        lockManager = MagicLockManager.instance;
    }
    public override void Interact() {
        lockManager.UnlockSkill(skillToUnlock);
    }
}
