using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicLockManager : MonoBehaviour
{
    public static MagicLockManager instance;
    public UnityEvent onMagicUnlocked;

    private void Awake() {
        instance = this;
    }
    
    //Make a interactable that can do this.
    public void UnlockSkill(MagicMoveSO unlockSkill) {
        unlockSkill.isUnlocked = true;
        onMagicUnlocked.Invoke();
    }
}
