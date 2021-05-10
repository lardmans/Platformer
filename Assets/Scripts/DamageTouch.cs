using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTouch : MonoBehaviour
{

    [SerializeField]
    float damageAmount;

    [SerializeField]
    float immuneTime;

    float nextPossibleDamageTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (damageAmount <= 0)
        {
            Debug.Log("DamageAmount is 0 on " + gameObject.name);
        }
        if (immuneTime <= 0)
        {
            Debug.Log("ImmuneTime is 0 on " + gameObject.name);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            if (Time.time >= nextPossibleDamageTime)
            {
                health.TakeDamage(damageAmount);
                nextPossibleDamageTime = Time.time + immuneTime;
            }
        }
    }
}
