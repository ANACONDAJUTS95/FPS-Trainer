using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText, ammoText;

    public Image damageEffect;
    public float damageAlpha = .25f, damageFadeSpeed = 2f;

    public GameObject pauseScreen;

    // End GameUI
    public GameObject endGameUI; // Reference to the EndGameUI GameObject
    public Text endGameEnemiesEliminatedText;
    public Text endGameBulletsUsedText;
    public Text endGameTotalTimeText;

    // Lose Screen
    public GameObject loseGameUI;
    public Text loseScreenEnemiesEliminatedText;
    public Text loseScreenBulletsUsedText;
    public Text loseScreenTotalTimeText;

    // Enemy Elimination Counter
    private int eliminatedEnemiesCount = 0;

    public void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (damageEffect.color.a != 0)
        {
            damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        }
    }

    //End GameUI
    public void SetEnemiesEliminated(int count)
    {
        endGameEnemiesEliminatedText.text = "Enemies Eliminated: " + count;
    }

    public void SetBulletsUsed(int count)
    {
        endGameBulletsUsedText.text = "Bullets Used/Fired: " + count;
    }

    public void SetTotalTime(float time)
    {
        endGameTotalTimeText.text = "Total Time to Finish: " + time.ToString("F2") + " seconds";
    }


    public void ShowLoseScreen(int bulletsUsed, float totalTime, int enemiesEliminated)
    {
        loseGameUI.SetActive(true); // Activate the lose screen canvas
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        // Update the text elements in the lose screen UI
        loseScreenEnemiesEliminatedText.text = "Enemies Eliminated: " + enemiesEliminated;
        loseScreenBulletsUsedText.text = "Bullets Used/Fired: " + bulletsUsed;
        loseScreenTotalTimeText.text = "Total Time to Finish: " + totalTime.ToString("F2") + " seconds";
    }



    public void UpdateEliminatedEnemiesCount()
    {
        eliminatedEnemiesCount++;
        loseScreenEnemiesEliminatedText.text = "Enemies Eliminated: " + eliminatedEnemiesCount;
    }

    public void UpdateBulletsUsed()
    {
        int bulletsUsed = BulletCounter.instance.bulletsFired;
        loseScreenBulletsUsedText.text = "Bullets Used/Fired: " + bulletsUsed;
    }

    private void SetLoseScreenText(int enemiesEliminated, int bulletsUsed, float totalTime)
    {
        loseScreenEnemiesEliminatedText.text = "Enemies Eliminated: " + enemiesEliminated;
        loseScreenBulletsUsedText.text = "Bullets Used/Fired: " + bulletsUsed;
        loseScreenTotalTimeText.text = "Total Time to Finish: " + totalTime.ToString("F2") + " seconds";
    }


    public void ShowEndGameUI(int enemiesEliminated, int bulletsUsed, float totalTime)
    {
        // Show the EndGameUI container
        endGameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        // Update the text elements in the end game UI
        endGameEnemiesEliminatedText.text = "Enemies Eliminated: " + enemiesEliminated;
        endGameBulletsUsedText.text = "Bullets Used/Fired: " + bulletsUsed;
        endGameTotalTimeText.text = "Total Time to Finish: " + totalTime.ToString("F2") + " seconds";
    }

    public void HideEndGameUI()
    {
        // Hide the EndGameUI container
        endGameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, .25f);
    }
}
