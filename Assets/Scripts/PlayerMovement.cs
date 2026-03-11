using System;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float maxJumpHeight;
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(horizontalMovement * Speed, 0);

        void FixedUpdate()
        {
            movement = new Vector2(horizontalMovement, 0).normalized;
            rb.linearVelocity = new Vector2(movement.x * Speed, movement.y * Speed) * Time.fixedDeltaTime;
        }
        //if(Input.GetKey(KeyCode.Space))
        //    rb.AddForce (Vector2.up * maxJumpHeight, 0);
    }
}