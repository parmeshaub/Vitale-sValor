using UnityEngine;

public class GateInteractable : Interactable
{
    [SerializeField] private GateManager gateManager;

    private void OnEnable() {
        if (gateManager != null && gateManager.gateOpened != null) {
            gateManager.gateOpened.AddListener(OpenGate);
        }
        else {
            Debug.LogError("GateManager or gateOpened event is null!");
        }
    }

    private void OnDisable() {
        if (gateManager != null && gateManager.gateOpened != null) {
            gateManager.gateOpened.RemoveListener(OpenGate);
        }
    }

    public override void Interact() {
        if (gateManager != null) {
            gateManager.TryOpenGate();
        }
        else {
            Debug.LogError("GateManager instance is null!");
        }
    }

    public void OpenGate() {
        Destroy(gameObject);
    }
}
