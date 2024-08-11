using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLoader : MonoBehaviour
{
    private SceneLoader sceneLoader;
    [SerializeField] private string sceneName;
    [SerializeField] private Transform exit;

    private void Start() {
        sceneLoader = SceneLoader.Instance;

        // Ensure sceneLoader is not null
        if (sceneLoader == null) {
            Debug.LogError("SceneLoader.Instance is null. Make sure SceneLoader is initialized.");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (sceneLoader != null && other.CompareTag("Player")) {
            sceneLoader.LoadDungeon(sceneName, exit);
        }
    }
}
