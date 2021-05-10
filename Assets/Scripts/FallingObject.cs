using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingObject : Triggerable
{

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator Activate(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

}
