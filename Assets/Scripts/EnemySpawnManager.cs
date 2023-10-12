using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float respawnTime;
    public int maxSpawnCount = 30; // Maximum number of enemy spawns

    private int spawnCount = 0;     // Variable for enemy spawns
    private int bulletsUsed;        // Variable to track bullets used
    private float totalTime;        // Variable for playtime

    private float startTime; // Variable to store the start time

    private UIController uiController;
    private bool gameEnded = false;

    private void Awake()
    {
        uiController = UIController.instance;
    }

    private void Start()
    {
        startTime = Time.time; // Record the start time

        // Start spawning enemies one by one
        StartCoroutine(SpawnEnemiesOneByOne());
    }

    private IEnumerator SpawnEnemiesOneByOne()
    {
        while (!gameEnded && spawnCount < maxSpawnCount)
        {
            // Get a random position within the map boundaries
            Vector3 randomPosition = GetRandomPositionWithinMap();

            // Spawn a new enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemy.SetActive(true);

            // Increment the spawn count
            spawnCount++;

            // Wait until the enemy is eliminated before spawning the next one
            while (enemy.activeInHierarchy)
            {
                yield return null;
            }
        }

        if (spawnCount >= maxSpawnCount)
        {
            // Calculate the total time when you meet the end condition
            totalTime = Time.time - startTime;
            gameEnded = true;

            UpdateUI();
        }
    }

    private Vector3 GetRandomPositionWithinMap()
    {
        // Replace these values with your map boundaries in 3D space
        float minX = -10f;
        float maxX = 10f;
        float minZ = -10f; // Minimum Z coordinate
        float maxZ = 10f; // Maximum Z coordinate

        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomZ = UnityEngine.Random.Range(minZ, maxZ);

        return new Vector3(randomX, 0, randomZ); // 3D coordinates
    }

    private void UpdateUI()
    {
        if (uiController != null)
        {
            // Display the end-game UI
            bulletsUsed = PlayerController.instance.bulletsFired; // Update bulletsUsed

            uiController.SetBulletsUsed(bulletsUsed); // Update bulletsUsed in the UI
            uiController.SetTotalTime(totalTime); // Update totalTime in the UI
            uiController.ShowEndGameUI(spawnCount, bulletsUsed, totalTime);
        }
    }
}
