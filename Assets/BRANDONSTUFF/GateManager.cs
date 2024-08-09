using UnityEngine;
using UnityEngine.Events;

public class GateManager : MonoBehaviour
{

    [SerializeField] private int requiredKeys = 2;
    private int keysCollected = 0;

    [HideInInspector] public UnityEvent gateOpened = new UnityEvent();

    private void Awake() {

    }

    public void CollectKey() {
        keysCollected++;
        Debug.Log("Key collected! Total keys: " + keysCollected);
    }

    public void TryOpenGate() {
        if (keysCollected >= requiredKeys) {
            OpenGate();
        }
        else {
            Debug.Log("Not enough keys. Keys needed: " + (requiredKeys - keysCollected));
        }
    }

    private void OpenGate() {
        // Logic to open the gate
        Debug.Log("The gate has been opened!");

        // Trigger the gateOpened event
        gateOpened.Invoke();
    }
}
