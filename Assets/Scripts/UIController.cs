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

    //End GameUI
    public GameObject endGameUI; // Reference to the EndGameUI GameObject
    public Text enemiesEliminatedText;
    public Text bulletsUsedText;
    public Text totalTimeText;

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
        enemiesEliminatedText.text = "Enemies Eliminated: " + count;
    }

    public void SetBulletsUsed(int count)
    {
        bulletsUsedText.text = "Bullets Used/Fired: " + count;
    }

    public void SetTotalTime(float time)
    {
        totalTimeText.text = "Total Time to Finish: " + time.ToString("F2") + " seconds";
    }

    public void ShowEndGameUI(int enemiesEliminated, int bulletsUsed, float totalTime)
    {
        // Show the EndGameUI container
        endGameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

        // Update the text elements with the provided information
        enemiesEliminatedText.text = "Enemies Eliminated: " + enemiesEliminated;
        bulletsUsedText.text = "Bullets Used/Fired: " + bulletsUsed;
        totalTimeText.text = "Total Time to Finish: " + totalTime.ToString("F2") + " seconds";
    }

    public void HideEndGameUI()
    {
        // Hide the EndGameUI container
        endGameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, .25f);
    }
}
