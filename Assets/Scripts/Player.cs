using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    [Header("Settings")]
    public KeyCode jumpKey;
    public float jumpHeight = 2f;
    public float timeToJumpApex = 0.5f;
    public float movementSpeed = 6f;
    public float accelerationTimeAirborne = 0.2f;
    public float accelerationTimeGrounded = 0.05f;

    // References
    Controller2D controller;
    Vector3 velocity;

    // Privates
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(jumpKey) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * movementSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne );
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
