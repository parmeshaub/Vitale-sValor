using UnityEngine;

public class GateManager : MonoBehaviour
{
    public static GateManager instance;

    [SerializeField] private int requiredKeys = 2; 
    private int keysCollected = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectKey()
    {
        keysCollected++;
        Debug.Log("Key collected! Total keys: " + keysCollected);
    }

    public void TryOpenGate()
    {
        if (keysCollected >= requiredKeys)
        {
            OpenGate();
        }
        else
        {
            Debug.Log("Not enough keys. Keys needed: " + (requiredKeys - keysCollected));
        }
    }

    private void OpenGate()
    {
        // Logic to open the gate
        Debug.Log("The gate has been opened!");
    }
}