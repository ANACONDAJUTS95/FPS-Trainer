using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to your enemy prefab
    public float spawnInterval = 5f; // Time interval for spawning enemies

    private Vector3 spawnAreaCenter = Vector3.zero; // Center of the spawn area
    public float spawnRadius = 12f; // Radius of the spawn area

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning enemies repeatedly at the specified interval
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Calculate a random spawn position within the specified area
            Vector3 randomSpawnPosition = spawnAreaCenter + Random.insideUnitSphere * spawnRadius;

            // Set the spawn position's Y-coordinate to the ground level (adjust as needed)
            randomSpawnPosition.y = 0f;

            // Instantiate an enemy at the random spawn position
            GameObject enemy = Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);

            // Activate the enemy
            enemy.SetActive(true);

            // Log the spawn position (for debugging)
            Debug.Log("Enemy spawned at " + randomSpawnPosition);

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
