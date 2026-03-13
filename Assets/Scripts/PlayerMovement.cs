using System;
using Unity.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]

    [SerializeField] public float Speed;
    [SerializeField] public float jumpForce;

    [Header("Ground Check Settings")]
    [SerializeField] public Transform GroundCheck;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody2D rb;
    private float xAxis;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInputs();
        Move();
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(Speed * xAxis, rb.linearVelocity.y);
    }

    public bool Grounded()
    {
        if(Physics2D.Raycast(GroundCheck.position, Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
    }
}