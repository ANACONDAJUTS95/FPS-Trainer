using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEnemy : MonoBehaviour
{
    private Vector3 originalPosition;
    public float respawnTime; // Time in seconds before respawn
    private bool isDead = false; // To track if the enemy is dead

    // References to the EnemyHealthController component and variables
    private EnemyHealthController healthController;
    private int currentHealth;
    private int maxHealth;

    void Awake()
    {
        // Get a reference to the EnemyHealthController component
        healthController = GetComponent<EnemyHealthController>();

        // Store the original spawn position
        originalPosition = transform.position;

        // Initialize other variables as needed
        if (healthController != null)
        {
            currentHealth = healthController.currentHealth;
            maxHealth = healthController.maxHealth;
        }
        else
        {
            Debug.LogWarning("EnemyHealthController component not found on the enemy.");
        }
    }

    void Start()
    {
        // Store the original spawn position
        originalPosition = transform.position;

        // Initialize other variables as needed
        // currentHealth = maxHealth;
    }

    public void HandleDeathAndRespawn()
    {
        // Check if the enemy is already dead
        if (!isDead)
        {
            // Set the enemy as dead
            isDead = true;

            // Deactivate the enemy GameObject
            gameObject.SetActive(false);

            // Start the respawn timer
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        // Reset the enemy's position to the original spawn position
        transform.position = originalPosition;

        // Reactivate the enemy GameObject
        gameObject.SetActive(true);

        // Reset any other necessary variables (e.g., health)
        currentHealth = maxHealth;

        // Set the enemy as not dead
        isDead = false;
    }
}
