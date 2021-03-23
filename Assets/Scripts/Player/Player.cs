using UnityEngine;

public class Player : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] Vector2 deathKick;

    //STATES
    string currentAnimation;
    bool isRunning;
    bool isGrounded;
    bool isTouchingLadder;
    bool isAlive = true;

    //CACHED REFERENCES
    Rigidbody2D rigidBody2D;
    Animator animator;
    CapsuleCollider2D BodyCollider;
    BoxCollider2D FeetCollider;

    float defaultGravity;

    //STRING REFERENCES
    static string HORIZONTAL_AXIS = "Horizontal";
    static string VERTICAL_AXIS = "Vertical";
    static string JUMP_BUTTON = "Jump";
    static string GROUND_LAYER = "Ground";
    static string LADDER_LAYER = "Ladder";
    static string ENEMY_LAYER = "Enemy";

    //ANIMATION STATES
    static string PLAYER_IDLE = "player_idle";
    static string PLAYER_RUNNING = "player_running";
    static string PLAYER_JUMPING_UP = "player_jump_up";
    static string PLAYER_JUMPING_DOWN = "player_jump_down";
    static string PLAYER_CLIMBING = "player_climbing";

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        BodyCollider = GetComponent<CapsuleCollider2D>();
        FeetCollider = GetComponent<BoxCollider2D>();
        defaultGravity = rigidBody2D.gravityScale;
    }

    private void Update()
    {
        ManageBools();
        ManageInputs();
        FlipPlayer();
        //UpdateAnimationState();
        Die();
        Debug.Log(isAlive);
    }

    //MANAGERS
    private void ManageBools()
    {
        isGrounded = FeetCollider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER));
        isRunning = Mathf.Abs(rigidBody2D.velocity.x) > 0; //Mathf.Abs is used to convert any negative value to a positive one
        isTouchingLadder = FeetCollider.IsTouchingLayers(LayerMask.GetMask(LADDER_LAYER));
    }

    private void Die()
    {
        if (BodyCollider.IsTouchingLayers(LayerMask.GetMask(ENEMY_LAYER)) && isAlive)
        {
            isAlive = false;
            rigidBody2D.velocity = deathKick;
        }
    }

    //INPUTS
    private void ManageInputs()
    {
        if (isAlive)
        {
            Run();
            ClimbLadders();
            Jump();
        }
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis(HORIZONTAL_AXIS); //varies between 1 and -1
        Vector2 runVelocity = new Vector2(controlThrow * runSpeed, rigidBody2D.velocity.y);
        rigidBody2D.velocity = runVelocity;
    }

    private void ClimbLadders()
    {
        float controlThrow = Input.GetAxis(VERTICAL_AXIS); //varies between 1 and -1
        if (isTouchingLadder)
        {
            rigidBody2D.gravityScale = 0;
            Vector2 climbVelocity = new Vector2(rigidBody2D.velocity.x, controlThrow * climbSpeed);
            rigidBody2D.velocity = climbVelocity;
        }
        else
        {
            rigidBody2D.gravityScale = defaultGravity;
        }

    }

    private void Jump()
    {
        if(isGrounded || isTouchingLadder)
        {
            if (Input.GetButtonDown(JUMP_BUTTON))
            {
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpForce);
            }
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
        animator.speed = 1;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    
    private void UpdateAnimationState()
    {
        if (isTouchingLadder)
        {
            if (rigidBody2D.velocity.y != 0)
            {
                ChangeAnimationState(PLAYER_CLIMBING);
            }
            else
            {
                animator.speed = 0;
            }
        }

        else if (!isGrounded && rigidBody2D.velocity.y <= 0)
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
