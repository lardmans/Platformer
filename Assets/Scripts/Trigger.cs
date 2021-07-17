using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [SerializeField]
    public TriggerSubject[] subjects;

    // Privates
    Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (subjects.Length > 0)
            {
                foreach (TriggerSubject ts in subjects)
                {
                    ITriggerable[] triggerables = ts.subject.GetComponents<ITriggerable>();

                    if (triggerables != null)
                    {
                        foreach (ITriggerable triggerable in triggerables)
                        {
                            StartCoroutine(triggerable.Activate(ts.delay));
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct TriggerSubject
    {
        public float delay;
        public GameObject subject;
    }
}
