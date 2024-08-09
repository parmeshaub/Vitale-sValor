using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonKeyInteractable : Interactable
{
    public override void Interact()
    {
        GateManager.instance.CollectKey();
        Destroy(gameObject); 
    }
}
