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
    public float respawnTime = 2f;
    public Vector3 spawnPositionOffset;

    // References
    Controller2D controller;
    Vector3 velocity;
    PlayerInput inputSystem;
    Health health;

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
    bool playerHasControl;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        inputSystem = GetComponent<PlayerInput>();
        health = GetComponent<Health>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        transform.position = SpawnManager.Singleton.currentSpawnPoint;
        playerHasControl = true;


    }

    // Update is called once per frame
    void Update()
    {
        if (! playerHasControl)
        {
            directionalInput = Vector2.zero;
        }

        if (controller.collisions.below)
        {
            canDoubleJump = true;
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

        float accelerationTime = (controller.collisions.below) ? (directionalInput.x == 0) ? accelerationTimeGrounded : 0 : (directionalInput.x == 0) ? accelerationTimeAirborne : 0;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);

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

    public void Die()
    {
        inputSystem.playerIsControlling = false;
        GameEvents.Singleton.Die();
        StartCoroutine(Respawn());
    }


    private IEnumerator Respawn()
    {
        Vector3 spawnPoint = SpawnManager.Singleton.currentSpawnPoint;
        spawnPoint += spawnPositionOffset;
        yield return new WaitForSeconds(respawnTime);
        GameEvents.Singleton.Respawn();
        transform.position = spawnPoint;
        velocity = Vector2.zero;
        inputSystem.playerIsControlling = true;
        health.Regenerate();
    }
}
