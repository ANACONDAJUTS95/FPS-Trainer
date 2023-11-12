using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float respawnTime;
    public int maxSpawnCount = 30; // Maximum number of enemy spawns

    private int spawnCount = 0; // Variable for enemy spawns
    private int bulletsUsed; // Variable to track bullets used
    private float totalTime; // Variable for playtime

    private float startTime; // Variable to store the start time

    private UIController uiController;
    private bool gameEnded = false;

    private int enemiesEliminated = 0;

    private EnemyCounterManager enemyCounterManager;

    private void Awake()
    {
        uiController = UIController.instance;
        enemyCounterManager = EnemyCounterManager.instance;
    }

    private void Start()
    {
        startTime = Time.time; // Record the start time

        // Start spawning enemies one by one
        StartCoroutine(SpawnEnemiesOneByOne());
    }

    private IEnumerator SpawnEnemiesOneByOne()
    {
        bool endGameUIShown = false; // Flag to track if the EndGameUI is shown

        while (!gameEnded && spawnCount < maxSpawnCount)
        {
            // Get a random position within the map boundaries
            Vector3 randomPosition = GetRandomPositionWithinMap();

            // Play the directional sound effect when an enemy is about to spawn
            PlayDirectionalSpawnSound(randomPosition);

            // Spawn a new enemy at the random position
            Debug.Log("enemyPrefab: " + enemyPrefab); // Check the reference
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            Debug.Log("enemy: " + enemy); // Check the instantiated enemy
            enemy.SetActive(true);

            // Increment the spawn count
            spawnCount++;

            // Wait until the enemy is eliminated before spawning the next one
            while (enemy.activeInHierarchy)
            {
                yield return null;
            }

            // Increment the enemiesEliminated count
            enemiesEliminated++;

            // Update the enemies eliminated count in the EnemyCounterManager
            enemyCounterManager.IncrementEnemiesEliminated();

            // Debug log to confirm when an enemy is eliminated.
            Debug.Log("Enemy Eliminated! Remaining: " + (maxSpawnCount - spawnCount));

            // Wait for the respawnTime before spawning the next enemy
            yield return new WaitForSeconds(respawnTime);
        }

        if (spawnCount >= maxSpawnCount && !endGameUIShown)
        {
            // Calculate the total time when you meet the end condition
            totalTime = Time.time - startTime;
            gameEnded = true;

            // Check if the maximum spawn count is reached
            Debug.Log("Spawn count reached maximum!");
            UpdateUI();
            endGameUIShown = true; // Set the flag to true to prevent multiple UI displays.
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
            bulletsUsed = BulletCounter.instance.bulletsFired;

            //bulletsUsed = PlayerController.instance.bulletsFired; // Update bulletsUsed
            //bulletsUsed = PlayerMovement.instance.bulletsFired; // Update bulletsUsed

            uiController.SetBulletsUsed(bulletsUsed); // Update bulletsUsed in the UI
            uiController.SetTotalTime(totalTime); // Update totalTime in the UI

            if (spawnCount >= maxSpawnCount) // Check if the maximum spawn count is reached
            {
                Debug.Log("Spawn count reached maximum!");

                uiController.ShowEndGameUI(spawnCount, bulletsUsed, totalTime);
            }

            // Update enemiesEliminated in the GameManager
            GameManager.instance.SetEnemiesEliminated(enemiesEliminated);
        }

        PlayerController.instance.footstepFast.Stop();
        PlayerController.instance.footstepSlow.Stop();
        PlayerMovement.instance.footstepFastGOAP.Stop();
        PlayerMovement.instance.footstepSlowGOAP.Stop();
    }

    private void PlayDirectionalSpawnSound(Vector3 spawnPosition)
    {
        // Calculate the direction from the spawn position to the player (assuming player is at the origin)
        Vector3 directionToPlayer = -spawnPosition.normalized;

        // Create a temporary game object at the spawn position
        GameObject soundSource = new GameObject("SpawnSoundSource");
        soundSource.transform.position = spawnPosition;

        // Add an AudioSource component to the temporary game object
        AudioSource audioSource = soundSource.AddComponent<AudioSource>();

        // Set the audio clip (assuming you have a spawn sound assigned to soundEffects[7])
        audioSource.clip = AudioManager.instance.soundEffects[7].clip;

        // Set the audio source properties for spatialization
        audioSource.spatialBlend = 1.0f; // Full 3D spatialization
        audioSource.minDistance = 1.0f; // Minimum distance for sound to be audible
        audioSource.maxDistance = 10.0f; // Maximum distance for sound to be audible

        // Set the direction of the sound to the player
        audioSource.transform.forward = directionToPlayer;

        // Play the sound
        audioSource.Play();

        // Destroy the temporary game object after the sound is played
        Destroy(soundSource, audioSource.clip.length);
    }

}
