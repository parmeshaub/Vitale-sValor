using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject playerPrefab; // Assign your player prefab here
    public Transform spawnPoint;    // Assign the initial spawn point

    private GameObject playerInstance;

    private bool doOnce = false;

    private void Awake() {
        // Implement Singleton pattern to ensure only one instance of PlayerManager exists
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        // Check if a player already exists
        if (playerInstance == null) {
            // Instantiate the player at the spawn point
            playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            DontDestroyOnLoad(playerInstance);
        }
        else if(!doOnce){
            // If the player already exists, just move them to the spawn point
            playerInstance.transform.position = spawnPoint.position;
            playerInstance.transform.rotation = spawnPoint.rotation;
            doOnce = true;
        }
        else {
            return;
        }
    }

    public void RemoveExistingPlayer() {
        if (playerInstance != null) {
            Destroy(playerInstance);
            playerInstance = null;
        }
    }
}
