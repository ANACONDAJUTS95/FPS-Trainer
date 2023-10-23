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
    public GameObject loseScreenCanvas;
    public Text loseScreenEnemiesEliminatedText;
    public Text loseScreenBulletsUsedText;
    public Text loseScreenTotalTimeText;


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
        Debug.Log("SetEnemiesEliminated called with count: " + count); // Add this line for debugging
        endGameEnemiesEliminatedText.text = "Enemies Eliminated: " + count;
        loseScreenEnemiesEliminatedText.text = "Enemies Eliminated: " + count;
    }

    public void SetBulletsUsed(int count)
    {
        endGameBulletsUsedText.text = "Bullets Used/Fired: " + count;
        loseScreenBulletsUsedText.text = "Bullets Used/Fired: " + count;
    }

    public void SetTotalTime(float time)
    {
        endGameTotalTimeText.text = "Total Time to Finish: " + time.ToString("F2") + " seconds";
        loseScreenTotalTimeText.text = "Total Time to Finish: " + time.ToString("F2") + " seconds";
    }

    public void ShowLoseScreen(int enemiesEliminated, int bulletsUsed, float totalTime)
    {
        loseScreenCanvas.SetActive(true); // Activate the lose screen canvas
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        // Update the text elements in the lose screen UI
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
