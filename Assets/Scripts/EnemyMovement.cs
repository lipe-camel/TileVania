using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float moveSpeed = 10f;

    //CACHED REFERENCES
    Rigidbody2D rigidBody2D;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Turn();
    }

    private void Move()
    {
        rigidBody2D.velocity = new Vector2(moveSpeed * transform.localScale.x, rigidBody2D.velocity.y);
    }

    private void Turn()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rigidBody2D.velocity.x)), 1f);
    }
}
