using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{

    public Triggerable[] subjects;
    public float delay = 0f;
    public bool repeatable = false;

    // Privates
    Collider2D col;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!activated)
            {
                activated = repeatable ? false : true;

                foreach (Triggerable subject in subjects)
                {
                    StartCoroutine(subject.Activate(delay));
                }
            }
        }
    }
}
