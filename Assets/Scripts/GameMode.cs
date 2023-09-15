using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public string headShot, bodyShot;

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
        SceneManager.LoadScene(headShot);
    }

    public void Bodyshot()
    {
        SceneManager.LoadScene(bodyShot);
    }

    public void HeadshotGOAP()
    {
        //SceneManager.LoadScene();
        Debug.Log("Not yet available");
    }

    public void BodyshotGOAP()
    {
        //SceneManager.LoadScene();
        Debug.Log("Not yet available");
    }
}
