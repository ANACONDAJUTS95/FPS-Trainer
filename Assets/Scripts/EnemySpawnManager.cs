using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float respawnTime = 1f;
    public int maxSpawnCount = 30; // Maximum number of enemy spawns

    private int spawnCount = 0;

    private void Start()
    {
        // Start spawning enemies
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Check if the spawnCount has reached the maximum limit
            if (spawnCount < maxSpawnCount)
            {
                // Check if there are any active enemies in the scene
                if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                {
                    // Get a random position within the map boundaries
                    Vector3 randomPosition = GetRandomPositionWithinMap();

                    // Spawn a new enemy at the random position
                    GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
                    enemy.SetActive(true);

                    // Increment the spawn count
                    spawnCount++;
                }
            }
            else
            {
                // Reached the maximum spawn limit, stop spawning
                Debug.Log("Maximum spawn limit reached.");
                yield break; // Exit the coroutine
            }

            // Wait for the specified respawnTime before checking again
            yield return new WaitForSeconds(respawnTime);
        }
    }

    private Vector3 GetRandomPositionWithinMap()
    {
        // Replace these values with your map boundaries in 3D space
        float minX = -10f;
        float maxX = 10f;
        float minZ = -10f; // Minimum Z coordinate
        float maxZ = 10f; // Maximum Z coordinate

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, 0, randomZ); // 3D coordinates
    }
}
