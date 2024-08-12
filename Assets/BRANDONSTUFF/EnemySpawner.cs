using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int initialCount = 5;
    public float spawnRadius = 5.0f;
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialCount; i++)
        {
            SpawnEnemyAtRandomPosition();
        }
        StartCoroutine(CheckAndRespawnEnemies());
    }

    void Update()
    {
        enemies.RemoveAll(enemy => enemy == null); // Automatically clean up null references in the list
    }

    IEnumerator CheckAndRespawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(300); // Adjusted to 30 seconds for demonstration

            int missingCount = initialCount - enemies.Count;
            for (int i = 0; i < missingCount; i++)
            {
                SpawnEnemyAtRandomPosition();
            }
        }
    }

    void SpawnEnemyAtRandomPosition()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = transform.position.y;
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.tag = "Enemy";
        enemies.Add(enemy);
    }
}
