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

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.endGameUI.activeInHierarchy)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpause();
            }
        }
    }

    public void PlayerDied(int enemiesEliminated, int bulletsUsed, float totalTime)
    {
        Debug.Log("PlayerDied method called!");

        Time.timeScale = 0f; // Pause the game

        // Log the values to the Unity Console
        Debug.Log("Player Died - Enemies Eliminated: " + enemiesEliminated);
        Debug.Log("Player Died - Bullets Used/Fired: " + bulletsUsed);
        Debug.Log("Player Died - Total Time to Finish: " + totalTime);

        // Pass the values to the UIController to display on the Lose Screen
        UIController.instance.ShowLoseScreen(enemiesEliminated, bulletsUsed, totalTime);
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
        }

        else
        {
            UIController.instance.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;

            
        }

        PlayerController.instance.footstepFast.Stop();
        PlayerController.instance.footstepSlow.Stop();
        PlayerMovement.instance.footstepFast.Stop();
        PlayerMovement.instance.footstepSlow.Stop();
    }
}
