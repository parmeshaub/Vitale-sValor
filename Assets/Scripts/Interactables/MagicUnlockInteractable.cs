using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicUnlockInteractable : Interactable
{
    [SerializeField] private MagicMoveSO skillToUnlock;
    private MagicLockManager lockManager;

    public UnityEvent onSkillUnlocked;

    private void Start() {
        lockManager = MagicLockManager.instance;
    }

    public override void Interact() {
        lockManager.UnlockSkill(skillToUnlock);

        // Invoke the Unity Event when the skill is unlocked
        if (onSkillUnlocked != null) {
            onSkillUnlocked.Invoke();
        }
    }
}
