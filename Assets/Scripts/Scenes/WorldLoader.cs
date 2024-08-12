using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private SceneLoader sceneLoader;
    private void Awake() {
        sceneLoader = SceneLoader.Instance;
    }
    private void OnTriggerEnter(Collider other) {
        sceneLoader.LoadToWorld(sceneName);
        GameManager.Instance.miniDungeonsCompleted++;
    }
}
