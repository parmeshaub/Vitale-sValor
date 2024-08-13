using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class FightingDungeonSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<GameObject> enemyPrefabs; // List of enemy prefabs to spawn
    public int totalEnemies = 30; // Total number of enemies to spawn
    public int maxEnemiesAtOnce = 6; // Maximum number of enemies allowed to fight at a time

    [Header("Spawn Settings")]
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnRadius = 2f; // Radius for random spawn offset

    [Header("Event Settings")]
    public UnityEvent OnAllEnemiesDefeated; // Event invoked when all enemies are defeated
    private bool doOnce = false; // Flag to ensure the event is only invoked once
    [SerializeField] private DialogueSO dialogue; // Reference to DialogueSO for dialogue initiation

    private int enemiesSpawned = 0; // Track the number of enemies spawned
    private List<GameObject> activeEnemies = new List<GameObject>(); // List of currently active enemies

    void Start() {
        SpawnEnemies();
    }

    void Update() {
        // Check if there are fewer active enemies than the maximum allowed
        if (activeEnemies.Count < maxEnemiesAtOnce && enemiesSpawned < totalEnemies) {
            SpawnEnemies();
        }

        // Remove null references from the activeEnemies list (in case an enemy was destroyed)
        activeEnemies.RemoveAll(enemy => enemy == null);

        // Check if all enemies are defeated
        if (activeEnemies.Count == 0 && enemiesSpawned >= totalEnemies && !doOnce) {
            OnAllEnemiesDefeated.Invoke();
            doOnce = true;
            // Initiate dialogue
            DialogueManager.instance.InitiateDialogue(dialogue);
        }
    }

    void SpawnEnemies() {
        // Calculate how many enemies need to be spawned
        int enemiesToSpawn = Mathf.Min(maxEnemiesAtOnce - activeEnemies.Count, totalEnemies - enemiesSpawned);

        for (int i = 0; i < enemiesToSpawn; i++) {
            // Randomly select an enemy prefab
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            // Randomly select a spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Calculate random offset for the spawn point (excluding the Y-axis)
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            // Spawn the enemy with the random offset
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position + randomOffset, spawnPoint.rotation);
            activeEnemies.Add(newEnemy);

            enemiesSpawned++;
        }
    }
}
