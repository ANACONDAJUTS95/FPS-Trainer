using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    // Add an event to notify when the enemy dies
    public delegate void EnemyDeathAction();
    public event EnemyDeathAction OnEnemyDeath;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // Notify that the enemy has died
            Die();
        }
    }

    private void Die()
    {
        // Deactivate the enemy GameObject
        gameObject.SetActive(false);

        // Trigger the respawn event
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath();
        }
    }
}
