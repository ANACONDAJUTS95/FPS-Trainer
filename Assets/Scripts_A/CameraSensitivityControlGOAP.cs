using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSensitivityControlGOAP : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public InputField sensitivityInputField;

    private void Start()
    {
        // Set the Input Field's text to the current mouse sensitivity when the game starts.
        sensitivityInputField.text = playerMovement.mouseSensitivity.ToString();
    }

    // This method is called when the player finishes editing the Input Field
    public void SetSensitivityFromInput()
    {
        if (sensitivityInputField != null)
        {
            // Parse the input text to a float
            if (float.TryParse(sensitivityInputField.text, out float mouseSensitivity))
            {
                // Set the player's camera sensitivity
                playerMovement.mouseSensitivity = mouseSensitivity;

                // Now you should save the sensitivity to PlayerPrefs or another persistent storage
                PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
                PlayerPrefs.Save();

                // Log to check if the value is being saved
                Debug.Log("Sensitivity saved: " + mouseSensitivity);
            }
        }
    }

    // Add this method to set sensitivity directly from other scripts
    public void SetSensitivity(float sensitivity)
    {
        sensitivityInputField.text = sensitivity.ToString();
        SetSensitivityFromInput();
    }
}
