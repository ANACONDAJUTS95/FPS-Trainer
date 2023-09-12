using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    public string cpName;
    public static List<CheckpointController> checkpoints = new List<CheckpointController>();

    private void Awake()
    {
        checkpoints.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_lastCheckpoint"))
        {
            string lastCheckpoint = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_lastCheckpoint");
            if (string.IsNullOrEmpty(lastCheckpoint))
            {
                // Randomly select a checkpoint from the list
                int randomIndex = Random.Range(0, checkpoints.Count);
                CheckpointController randomCheckpoint = checkpoints[randomIndex];
                PlayerController.instance.transform.position = randomCheckpoint.transform.position;
                Physics.SyncTransforms();
                Debug.Log("Player starting at a random checkpoint.");
            }
            else if (lastCheckpoint == cpName)
            {
                PlayerController.instance.transform.position = transform.position;
                Physics.SyncTransforms();
                Debug.Log("Player starting at " + cpName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_lastCheckpoint", "");
        }
    }

    
}
