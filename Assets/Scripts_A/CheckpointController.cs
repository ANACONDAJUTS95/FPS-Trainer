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

    // Function to choose a random checkpoint and set the player's position
    public void SpawnPlayerRandomly()
    {
        int randomIndex = Random.Range(0, checkpoints.Count);
        CheckpointController randomCheckpoint = checkpoints[randomIndex];
        PlayerController.instance.transform.position = randomCheckpoint.transform.position;
        Physics.SyncTransforms();
        Debug.Log("Player starting at a random checkpoint.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
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