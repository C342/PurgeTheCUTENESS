using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]

    [SerializeField] public float Speed;
    [SerializeField] public float jumpForce;
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;

    [Header("Ground Check Settings")]

    [SerializeField] public Transform GroundCheck;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Header("Player Camera Settings")]
    [SerializeField] public Transform playerCamera;

    PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAxis;
    private float gravity;
    Animator anim;
    private bool canDash;

    public static PlayerMovement Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    bool attack = false;
    float timeBetweenAttack, timeSinceAttack;

    private void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;
    }

    private void Update()
    {
        GetInputs();
        UpdateJumpVariable();
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

    void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {

        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

            pState.jumping = false;
        }

        if (!pState.jumping)
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);

                pState.jumping = true;
            }
        }
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);

            pState.jumping = true;
        }
    }

    void UpdateJumpVariable()
    {
        if (Grounded())
        {
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            jumpBufferCounter--;
        }
    }
}