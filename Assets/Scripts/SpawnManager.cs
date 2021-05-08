using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Singleton;
    public Transform firstSpawnPoint;
    public Vector3 currentSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;

        if (firstSpawnPoint != null)
        {
            currentSpawnPoint = firstSpawnPoint.position;
        }
        else
        {
            currentSpawnPoint = new Vector3(-10, 4, 0);
        }
    }


}
