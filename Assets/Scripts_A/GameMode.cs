using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public string headShot, bodyShot, headShotGOAP, bodyShotGOAP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Headshot()
    {
        Time.timeScale = 1f; // Reset the time scale
        SceneManager.LoadScene(headShot);
    }

    public void Bodyshot()
    {
        Time.timeScale = 1f; // Reset the time scale
        SceneManager.LoadScene(bodyShot);
    }

    public void HeadshotGOAP()
    {
        Time.timeScale = 1f; // Reset the time scale
        SceneManager.LoadScene(headShotGOAP);
        //Debug.Log("Not yet available");
    }

    public void BodyshotGOAP()
    {
        Time.timeScale = 1f; // Reset the time scale
        SceneManager.LoadScene(bodyShotGOAP);
        //Debug.Log("Not yet available");
    }
}
