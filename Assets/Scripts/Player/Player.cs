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
        Move();
        Jump();
    }

    private void Move()
    {
        var deltaX = Input.GetAxis(HORIZONTAL_AXIS) * Time.deltaTime * moveSpeed;
        transform.position = new Vector2(transform.position.x + deltaX, transform.position.y);
        ChangeAnimationState(PLAYER_RUNNING);

        var currentXPosition = transform.position.x;
        if(transform.position.x > currentXPosition)
        {
            Debug.Log(transform.position.x - currentXPosition);
            transform.rotation = new Quaternion(0f, 0, 0f, 0f);
            currentXPosition = transform.position.x;
        }
        else if (transform.position.x < currentXPosition)
        {
            Debug.Log(transform.position.x - currentXPosition);
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            currentXPosition = transform.position.x;
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
