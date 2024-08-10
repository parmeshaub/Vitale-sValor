using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ArtemisPillar : Interactable
{
    public Animator artemisAnimator;
    public bool isUnlocked = false;

    public void ActivateArtemis()
    {
        artemisAnimator.SetTrigger("activated");
    }
 
    private void Update()
    {

    }

    public override void Interact() {
        isUnlocked = true;
        ActivateArtemis();
    }
}
