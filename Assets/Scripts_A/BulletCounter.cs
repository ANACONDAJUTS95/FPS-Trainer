using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCounter : MonoBehaviour
{
    public int bulletsFired;

    public static BulletCounter instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of BulletCounter found!");
            return;
        }
        instance = this;
    }
}
