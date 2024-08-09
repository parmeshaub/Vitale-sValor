using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonKeyInteractable : Interactable
{
    [SerializeField] private GateManager gateManager;
    public override void Interact()
    {
        gateManager.CollectKey();
        Destroy(gameObject); 
    }
}
