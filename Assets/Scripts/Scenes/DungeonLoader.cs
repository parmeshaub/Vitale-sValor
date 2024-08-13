using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLoader : MonoBehaviour
{
    private SceneLoader sceneLoader;
    [SerializeField] private string sceneName;
    [SerializeField] private Transform exit;
    public WorldDataStore worldDataStore;

    private void Start() {
        sceneLoader = SceneLoader.Instance;

        // Ensure sceneLoader is not null
        if (sceneLoader == null) {
            Debug.LogError("SceneLoader.Instance is null. Make sure SceneLoader is initialized.");
        }

        // Ensure worldDataStore is set
        if (worldDataStore == null) {
            worldDataStore = WorldDataStore.instance;
        }
    }

    private void OnTriggerEnter(Collider other) {
        sceneLoader = SceneLoader.Instance;
        if (sceneLoader != null && other.CompareTag("Player")) {
            // Update world data based on the dungeon
            UpdateWorldData(sceneName);

            // Load the dungeon scene
            sceneLoader.LoadDungeon(sceneName, exit);
        }
    }

    private void UpdateWorldData(string sceneName) {
        Debug.Log("call");
        switch (sceneName) {
            case "Dungeon_Ablaze":
                worldDataStore.ablazeCompleted = true;
                break;
            case "Dungeon_Judgement":
                worldDataStore.judgementCompleted = true;
                break;
            case "Dungeon_Blistfulness":
                worldDataStore.blistfulnessCompleted = true;
                break;
            case "Dungeon_Glaciate":
                worldDataStore.glaciateCompleted = true;
                break;
            case "Dungeon_Volley":
                worldDataStore.volleyCompleted = true;
                break;
            case "Dungeon_RazorFangs":
                worldDataStore.razorFangCompleted = true;
                break;
            case "Dungeon_WingsOfComfort":
                worldDataStore.wocCompleted = true;
                break;
            case "Dungeon_Combustion":
                worldDataStore.combustCompleted = true;
                break;
            case "Dungeon_Sanctuary":
                worldDataStore.sanctCompleted = true;
                break;
            case "04 - Dragon":
                worldDataStore.dragonBoss = true;
                break;
            case "05 - Boar":
                worldDataStore.boarBoss = true;
                break;
            default:
                Debug.LogWarning("Unknown dungeon scene name: " + sceneName);
                break;
        }
    }

}
