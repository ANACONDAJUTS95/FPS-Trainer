using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounterManager : MonoBehaviour
{
    // Variables to track the number of enemies and enemies eliminated
    public int totalEnemies; // Total number of enemies spawned
    public int enemiesEliminated; // Number of enemies eliminated

    public static EnemyCounterManager instance; // Singleton instance

    [Header("Enemy Count")]
    [SerializeField]
    private int eliminatedEnemiesCount; // Serialized field for eliminated enemies count

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of EnemyCounterManager found!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // Initialize the counters
        totalEnemies = 0;
        enemiesEliminated = 0;
    }

    // Method to increment the total enemies count
    public void IncrementTotalEnemies()
    {
        totalEnemies++;
    }

    // Method to increment the eliminated enemies count
    public void IncrementEnemiesEliminated()
    {
        enemiesEliminated++;
        eliminatedEnemiesCount = enemiesEliminated; // Update the serialized field
    }

    // Method to get the current count of eliminated enemies
    public int GetEnemiesEliminatedCount()
    {
        return enemiesEliminated;
    }

    // Method to get the total number of enemies spawned
    public int GetTotalEnemiesCount()
    {
        return totalEnemies;
    }
}
