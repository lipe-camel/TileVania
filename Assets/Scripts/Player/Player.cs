using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 1f;

    //STATES
    string currentAnimation;
    bool isRunning;

    //CACHED REFERENCES
    Rigidbody2D rigidBody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;

    //STRING REFERENCES
    static string HORIZONTAL_AXIS = "Horizontal";
    static string JUMP_AXIS = "Jump";

    //ANIMATION STATES
    static string PLAYER_IDLE = "player_idle";
    static string PLAYER_RUNNING = "player_running";

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis(HORIZONTAL_AXIS); //varies between 1 and -1
        Vector2 playerVelocity = new Vector2
            (controlThrow * moveSpeed, rigidBody2D.velocity.y);
        rigidBody2D.velocity = playerVelocity;

        bool isRunning = Mathf.Abs(rigidBody2D.velocity.x) > 0; //Mathf.Abs is used to convert any negative value to a positive one

        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody2D.velocity.x), 1f); //Mathf.Sign returns a value of 1 or -1
            ChangeAnimationState(PLAYER_RUNNING);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    private void Jump()
    {
        //var deltaY = Input.GetAxis(JUMP_AXIS) * Time.deltaTime * jumpForce;
        //transform.position = new Vector2(transform.position.x, transform.position.y + deltaY);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody2D.velocity = new Vector2(0, jumpForce);
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if(currentAnimation == newAnimation) { return; }
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
