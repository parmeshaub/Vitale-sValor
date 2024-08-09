using UnityEngine;

public class GateInteractable : Interactable
{
    public override void Interact()
    {
        GateManager.instance.TryOpenGate();
    }
}