using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject Player;
    public float PlaceX, PlaceZ;

    // Start is called before the first frame update
    void Start()
    {
        PlaceX = Random.Range(-12, 12);
        PlaceZ = Random.Range(-12, 12);

        Player.transform.position = new Vector3(PlaceX, 0, PlaceZ);
        Debug.Log("Player spawn at " + Player.transform.position);
    }
}
