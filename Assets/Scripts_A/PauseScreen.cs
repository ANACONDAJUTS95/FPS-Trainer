using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public string mainMenuScene;

    //Game Sensitivity
    public CameraSensitivityControl cameraSensitivityControl;
    public float desiredSensitivity;
    private float initialSensitivity;
    public InputField sensitivityInputField;


    // Start is called before the first frame update
    void Start()
    {

        // Initialize the initialSensitivity variable with the current sensitivity from PlayerPrefs
        initialSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        GameManager.instance.PauseUnpause();

        // Update the sensitivity input field
        float desiredSensitivity;
        if (float.TryParse(sensitivityInputField.text, out desiredSensitivity))
        {
            // Set the sensitivity input field
            sensitivityInputField.text = desiredSensitivity.ToString();

            // Apply the desired sensitivity
            cameraSensitivityControl.SetSensitivity(desiredSensitivity);
        }
        else
        {
            // If parsing fails, set the sensitivity to the initial value
            cameraSensitivityControl.SetSensitivity(initialSensitivity);
            sensitivityInputField.text = initialSensitivity.ToString();
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f; // Reset the time scale
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
