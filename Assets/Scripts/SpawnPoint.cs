using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    BoxCollider2D col;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            if (SpawnManager.Singleton.currentSpawnPoint != this.transform.position)
            {
                SpawnManager.Singleton.currentSpawnPoint = this.transform.position;
                audio.Play();
            }
            
        }
    }
}
