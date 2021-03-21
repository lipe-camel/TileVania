using UnityEngine;

public class Player : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 1f;

    //STATES
    string currentAnimation;
    bool isRunning;
    bool isGrounded;

    //CACHED REFERENCES
    Rigidbody2D rigidBody2D;
    Animator animator;
    Collider2D coll2D;

    //STRING REFERENCES
    static string HORIZONTAL_AXIS = "Horizontal";
    static string JUMP_BUTTON = "Jump";
    static string GROUND_LAYER = "Ground";

    //ANIMATION STATES
    static string PLAYER_IDLE = "player_idle";
    static string PLAYER_RUNNING = "player_running";
    static string PLAYER_JUMPING_UP = "player_jump_up";
    static string PLAYER_JUMPING_DOWN = "player_jump_down";

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        ManageBools();
        ManageInputs();
        FlipPlayer();
        UpdateAnimationState();
    }

    //MANAGERS
    private void ManageBools()
    {
        isGrounded = coll2D.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER));
        isRunning = Mathf.Abs(rigidBody2D.velocity.x) > 0; //Mathf.Abs is used to convert any negative value to a positive one
    }


    //INPUTS
    private void ManageInputs()
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
    }

    private void Jump()
    {

        if (Input.GetButtonDown(JUMP_BUTTON) && isGrounded)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpForce);
        }
    }


    //ANIMATION
    private void FlipPlayer()
    {
        if (isRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody2D.velocity.x), 1f); //Mathf.Sign returns a value of 1 or -1
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if(currentAnimation == newAnimation) { return; }
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    
    private void UpdateAnimationState()
    {
        if (!isGrounded && rigidBody2D.velocity.y <= 0)
        {
            ChangeAnimationState(PLAYER_JUMPING_DOWN);
        }
        else if (!isGrounded && rigidBody2D.velocity.y > 0)
        {
            ChangeAnimationState(PLAYER_JUMPING_UP);
        }

        else if (isRunning && isGrounded)
        {
            ChangeAnimationState(PLAYER_RUNNING);
        }
        else if (!isRunning && isGrounded)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

}
