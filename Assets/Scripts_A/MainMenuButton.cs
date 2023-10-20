using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // Attach this script to your Main Menu button in the Inspector.
    public Button mainMenuButton;

    void Start()
    {
        // Add an event listener to the button's onClick event.
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void LoadMainMenu()
    {
        // Load the Main Menu scene when the button is clicked.
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your actual scene name.
    }
}
