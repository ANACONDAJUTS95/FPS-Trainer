using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance of this object exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Register a callback for the scene loading event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is a game level or scene
        // You can use scene.name or other criteria to identify your game levels
        bool isGameScene = scene.name == "Bodyshot" || scene.name == "Headshot" || scene.name == "BodyshotGOAP" || scene.name == "HeadshotGOAP";

        // Stop the background music if a game scene is loaded
        if (isGameScene)
        {
            audioSource.Stop();
        }
        else
        {
            // If it's not a game scene, start or resume the background music
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnDestroy()
    {
        // Unregister the scene loading callback when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
