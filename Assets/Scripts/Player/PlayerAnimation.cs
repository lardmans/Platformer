using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    Animator animator;
    Controller2D controller;
    SpriteRenderer sprite;
    Health health;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("velocityY", controller.velocity.y);
        animator.SetFloat("velocityX", Mathf.Abs(controller.velocity.x));
        animator.SetBool("grounded", controller.collisions.below);
        
        if (Mathf.Abs(controller.velocity.x) > 0.01)
        {
            sprite.flipX = Mathf.Sign(controller.velocity.x) == -1;
        }

        animator.SetBool("dead", !health.alive);
    }
}
