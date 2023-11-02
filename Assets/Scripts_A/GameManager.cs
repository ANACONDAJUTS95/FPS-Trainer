using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float waitAfterDying = 2f;

    public int enemiesEliminated;
    public int bulletsUsed;
    public float totalTime;
    private float startTime; // Add this line


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startTime = Time.time; // Record the start time
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.endGameUI.activeInHierarchy && !UIController.instance.loseGameUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpause();
            }
        }
    }

    public void SetEnemiesEliminated(int count)
    {
        enemiesEliminated = count; // Update the local count
        UIController.instance.SetEnemiesEliminated(count); // Update the UI
    }

    public void UpdateEnemiesEliminated(int count)
    {
        enemiesEliminated = count;
        UIController.instance.SetEnemiesEliminated(count);
    }



    public void PlayerDied(int bulletsUsed, float totalTime)
    {
        totalTime = Time.time - startTime;
        bulletsUsed = BulletCounter.instance.bulletsFired; // Update bulletsUsed

        int enemiesEliminated = EnemyCounterManager.instance.GetEnemiesEliminatedCount(); // Get the count from your manager script

        UIController.instance.ShowLoseScreen(bulletsUsed, totalTime, enemiesEliminated); // Pass the count to ShowLoseScreen

        Time.timeScale = 0f;
    }



    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f; // Reset the time scale
    }

    public void PauseUnpause()
    {
        if (UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

            PlayerController.instance.footstepFast.Play();
            PlayerController.instance.footstepSlow.Play();
            PlayerMovement.instance.footstepFastGOAP.Play();
            PlayerMovement.instance.footstepSlowGOAP.Play();
        }
        else
        {
            UIController.instance.pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            PlayerController.instance.footstepFast.Stop();
            PlayerController.instance.footstepSlow.Stop();
            PlayerMovement.instance.footstepFastGOAP.Stop();
            PlayerMovement.instance.footstepSlowGOAP.Stop();

        }

        
    }

}
