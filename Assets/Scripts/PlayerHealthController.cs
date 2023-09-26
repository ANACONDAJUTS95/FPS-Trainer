using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int maxHealth, currentHealth;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damageAmount)
    {
        //Player is hurt
        int randomIndex = Random.Range(0, 3); // Generate a random index (0 to 2 for hurt sounds)
        AudioManager.instance.PlaySFX(randomIndex);

        currentHealth -= damageAmount;

        UIController.instance.ShowDamage();

        //Funtcion so that health won't go below 0
        //currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);

            currentHealth = 0;
            GameManager.instance.PlayerDied();

            AudioManager.instance.StopBGM();
            
            //Player Dead SFX
            AudioManager.instance.PlaySFX(3);

            //Stop soundeffect hurt so that it won't overlap with Dead sfx
            AudioManager.instance.StopSFX(randomIndex);
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }
}
