using UnityEngine;

public class MagicCoroutineHelper : MonoBehaviour
{
    public static MagicCoroutineHelper Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this object persists across scenes if needed
        }
        else {
            Destroy(gameObject);
        }
    }
}
