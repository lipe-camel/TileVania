using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float moveSpeed = 10f;

    //STATS
    bool haveGroundAhead;

    //CACHED REFERENCES
    Rigidbody2D rigidBody2D;
    BoxCollider2D groundChecker;

    //STRING REFERENCES
    static string GROUND_LAYER = "Ground";

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        ManageMovement();
        ManageBools();
        Debug.Log(haveGroundAhead); 
    }

    private void ManageBools()
    {
        haveGroundAhead = groundChecker.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER));
    }


    private void ManageMovement()
    {
        if (haveGroundAhead)
        {
            Move(1);
        }
        else
        {
            Move(-1);
        }
    }

    private void Move(int direction)
    {
        transform.localScale = new Vector2(direction, 1f);
        rigidBody2D.velocity = new Vector2(moveSpeed * direction, rigidBody2D.velocity.y);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
