using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistPlayerPack : MonoBehaviour
{
    public static PersistPlayerPack Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            // If no instance exists, this becomes the instance and persists
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this) {
            // If an instance already exists, destroy the new one
            Destroy(gameObject);
        }
    }
}
