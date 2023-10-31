using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EliminatedEnemiesCounter : MonoBehaviour
{
    public Text counterText; // Reference to the UI Text element
    public EnemyCounterManager enemyCounterManager; // Reference to the EnemyCounterManager script

    private void Start()
    {
        UpdateCounterText(); // Update the counter when the game starts
    }

    private void Update()
    {
        // Optionally, you can update the counter in real-time during the game.

        UpdateCounterText(); // Update the counter when the game starts
    }

    // Method to update the UI counter text
    public void UpdateCounterText()
    {
        if (counterText != null && enemyCounterManager != null)
        {
            int eliminatedCount = enemyCounterManager.GetEnemiesEliminatedCount();
            int totalEnemies = enemyCounterManager.GetTotalEnemiesCount();
            counterText.text = "Eliminated Enemies: " + eliminatedCount;
        }
    }
}
