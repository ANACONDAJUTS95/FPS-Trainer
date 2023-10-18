using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgm;

    public AudioSource[] soundEffects;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopBGM()
    {
        bgm.Stop();
    }

    public void PlaySFX(int sfxNumber)
    {
        if (sfxNumber >= 0 && sfxNumber < soundEffects.Length)
        {
            soundEffects[sfxNumber].Stop();
            soundEffects[sfxNumber].Play();
        }
        else
        {
            Debug.LogError("Invalid sound effect index: " + sfxNumber);
        }
    }


    public void StopSFX(int sfxNumber)
    {
        soundEffects[sfxNumber].Stop();
    }
}
