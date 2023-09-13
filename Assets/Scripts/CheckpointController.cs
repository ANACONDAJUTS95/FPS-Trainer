using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    public string cpName;
    public static List<CheckpointController> checkpoints = new List<CheckpointController>();

    private bool isDestroyed = false; // Flag to track if the object is destroyed

    private void Awake()
    {
        checkpoints.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isDestroyed && PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_lastCheckpoint"))
        {
            string lastCheckpoint = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_lastCheckpoint");
            if (string.IsNullOrEmpty(lastCheckpoint))
            {
                // Randomly select a checkpoint from the list
                int randomIndex = Random.Range(0, checkpoints.Count);
                CheckpointController randomCheckpoint = checkpoints[randomIndex];
                PlayerController.instance.transform.position = randomCheckpoint.transform.position;
                Physics.SyncTransforms();
                Debug.Log("Player respawning at a random checkpoint.");
            }
            else if (lastCheckpoint == cpName)
            {
                PlayerController.instance.transform.position = transform.position;
                Physics.SyncTransforms();
                Debug.Log("Player respawning at " + cpName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed && Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_lastCheckpoint", "");
        }
    }

    // Handle object destruction
    private void OnDestroy()
    {
        isDestroyed = true;
    }
}
