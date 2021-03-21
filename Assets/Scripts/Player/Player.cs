using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //INSPECTOR REFERENCES
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 1f;

    //STATES
    string currentAnimation;
    bool isRunning;

    //CACHED REFERENCES
    Rigidbody2D rigidBody2D;
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
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis(HORIZONTAL_AXIS);                         //varies between 1 and -1
        Vector2 playerVelocity = new Vector2
            (controlThrow * moveSpeed, rigidBody2D.velocity.y);
        rigidBody2D.velocity = playerVelocity;

        if(controlThrow < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (controlThrow > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);

        }

        if (controlThrow == 0)   { isRunning = false; }
        else                    { isRunning = true; }

        if (isRunning)
        {
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
