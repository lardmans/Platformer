using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingObject : RoomBehavior, ITriggerable
{
    // Settings
    [SerializeField]
    float gravityScale = 1;

    // Privates
    Rigidbody2D rb;
    bool hasBeenTriggered = false;

    private void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.gravityScale = gravityScale;
        SetDefaultValues();
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public override void SetDefaultValues()
    {
        base.SetDefaultValues();
    }

    public IEnumerator Activate(float delay)
    {
        if (! hasBeenTriggered)
        {
            hasBeenTriggered = true;
            yield return new WaitForSeconds(delay);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public override void Renew()
    {
        base.Renew();

        transform.position = defaultPosition;
        hasBeenTriggered = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

}
