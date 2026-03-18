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

    [Header("Player Camera Settings")]
    [SerializeField] public Transform playerCamera;

    private Rigidbody2D rb;
    private float xAxis;

    bool attack = false;
    float timeBetweenAttack, timeSinceAttack;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInputs();
        Move();
        Jump();
    }

    void CameraTransform(Transform playerCamera)
    {
        Transform child = playerCamera.GetChild(0);
        child.SetParent(playerCamera, false);
    }

    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        attack = Input.GetMouseButton(0);
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(Speed * xAxis, rb.linearVelocity.y);
    }

    public bool Grounded()
    {
        if (Physics2D.Raycast(GroundCheck.position, Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(GroundCheck.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround)
            || Physics2D.Raycast(GroundCheck.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
        }
    }
}