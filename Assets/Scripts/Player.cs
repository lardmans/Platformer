using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    [Header("Settings")]
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = 0.5f;
    public float movementSpeed = 6f;
    public float accelerationTimeAirborne = 0.2f;
    public float accelerationTimeGrounded = 0.05f;
    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = 0.25f;
    public Vector2 wallJumpClimb, wallJumpOff, wallLeap;

    // References
    Controller2D controller;
    Vector3 velocity;

    // Privates
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    float timeToWallUnstick;
    Vector2 directionalInput;

    bool wallSliding;
    bool canDoubleJump;
    int wallDirX;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        mat = GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisions.below)
        {
            canDoubleJump = true;
        }

        if (canDoubleJump)
        {
            mat.color = Color.blue;
        }
        else
        {
            mat.color = Color.red;
        }
        
        CalculateVelocity();

        //HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * movementSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        //velocity.x = targetVelocityX;
        velocity.y += gravity * Time.deltaTime;
    }

    void HandleWallSliding()
    {
        wallDirX = controller.collisions.left ? -1 : 1;
        wallSliding = false;

        if ((controller.collisions.left || controller.collisions.right) && (!controller.collisions.below && velocity.y < 0))
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                Debug.Log("Jumping climb");
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                Debug.Log("Jumping off");
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                Debug.Log("Jumping leap");
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        } 
        
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                {
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
            }
        }
        else if (canDoubleJump)
        {
            velocity.y = maxJumpVelocity;
            canDoubleJump = false;
        }
                
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }  
    }
}
