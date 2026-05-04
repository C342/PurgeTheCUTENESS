using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]

    [SerializeField] public float Speed;
    [SerializeField] public float jumpForce;
    private float jumpBufferCounter = 0;
    [SerializeField] private float jumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float attackMoveMultiplier = 0.4f;
    private int facingDirection = 1;
    private Vector3 originalScale;

    [Header("Ground Check Settings")]

    [SerializeField] public Transform GroundCheck;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Header("Player Camera Settings")]
    [SerializeField] public Transform playerCamera;

    [Header("Attack Settings")]
    bool attack = false;
    float timeSinceAttack = 0;
    [SerializeField] float timeBetweenAttack = 0.4f;
    [SerializeField] Transform SideAttackTransform, UpAttackTransform, DownAttackTransform;
    [SerializeField] Vector2 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] LayerMask attackableLayer;
    [SerializeField] float damage;

    [Header("Recoil Settings")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilYSpeed = 100;
    [SerializeField] float recoilXSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;

    [Header("Arm Rip Settings")]
    [SerializeField] private float multiplierIfRipped = 2f;
    private bool armRipped = false;
    private bool ripArmInput;

    [Header("Health Settings")]
    public int health = 100;
    public int maxHealth = 100;

    [HideInInspector] public PlayerStateList pState;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float xAxis, yAxis;
    private float gravity;
    Animator anim;
    private bool canDash = true;
    private bool dashed;

    public static PlayerController Instance;
    
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

    private void Start()
    {
        pState = GetComponent<PlayerStateList>();

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();

        gravity = rb.gravityScale;

        originalScale = transform.localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    void Recoil()
    {
        if (pState.recoilingX)
        {
            if (pState.lookingRight)
            {
                rb.linearVelocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.linearVelocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pState.recoilingY)
        {
            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, recoilYSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -recoilYSpeed);
            }
        }
        else
        {
            rb.gravityScale = gravity;
        }

        if (pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {

        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
    }

    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pState.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pState.recoilingY = false;
    }

    public void TakeDamage(float _damage)
    {
        health -= Mathf.RoundToInt(_damage);
        StartCoroutine(StopTakingDamage());
    }
    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;
        yield return new WaitForSeconds(1f);
        pState.invincible = false;
        // i-frame implementation
    }

    public int Health
{
    get { return health; }
    set
    {
        if (health != value)
        {
            health = Mathf.Clamp(value, 0, maxHealth);
        }
    }
}

    private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength, float _damage)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<BaseEnemyClass>() != null)
            {
                objectsToHit[i].GetComponent<BaseEnemyClass>().EnemyHit(_damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
            }
        }
    }

    void Update()
    {
        GetInputs();
        UpdateJumpVariable();

        if (pState.dashing || pState.locked) return;
        if(ripArmInput && !armRipped)
        {
            StartCoroutine(RipArmRoutine());
        }

        Attack();
        Flip();
        Move();
        Jump();
        StartDash();
    }

    public void EndAttack()
    {
        pState.attacking = false;
    }   

    void CameraTransform(Transform playerCamera)
    {
        Transform child = playerCamera.GetChild(0);
        child.SetParent(playerCamera, false);
    }

    private void FixedUpdate()
    {
        {
            if (pState.locked)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0;
                return;
            }

            rb.gravityScale = gravity;
            Recoil();
        }
    }

    void GetInputs()
    {
        if(pState.locked)
        {
            yAxis = 0;
            xAxis = 0;
            attack = false;
            return;
        }

        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");
        ripArmInput = Input.GetKeyDown(KeyCode.R);
    }

    private void Move()
{
    if (pState.locked)
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        return;
    }

    float currentSpeed = Speed;

    if (pState.attacking)
    {
        currentSpeed *= attackMoveMultiplier;
    }

    rb.linearVelocity = new Vector2(currentSpeed * xAxis, rb.linearVelocity.y);

    anim.SetBool("Walking", Mathf.Abs(rb.linearVelocity.x) > 0.1f && Grounded() && !pState.attacking);
}

    void StartDash()
    {
        if (pState.locked) return;
        
        if (Input.GetButtonDown("Dash") && canDash && !dashed)
        {
            StartCoroutine(Dash());
            dashed = true;
        }
        if (Grounded())
        {
            dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator RipArmRoutine()
    {
        pState.locked = true;
        armRipped = true;
        
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("RipArm");

        yield return new WaitForSeconds(1.5f);

        pState.locked = false;
    }

    public void EndRipArm()
    {
        pState.locked = false;
    }

    void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            facingDirection = -1;
            pState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            facingDirection = 1;
            pState.lookingRight = true;
        }
    }
    void Attack()
    {
        if(armRipped) return;
        {
            timeSinceAttack += Time.deltaTime;

            if (attack && timeSinceAttack >= timeBetweenAttack && !pState.attacking)
            {
                timeSinceAttack = 0;
                StartCoroutine(AttackRoutine());
            }
        }

        if(armRipped)
        {
            rippedAttack();
            return;
        }
    }

    void rippedAttack()
    {
        timeSinceAttack += Time.deltaTime;

        if (attack && timeSinceAttack >= timeBetweenAttack && !pState.attacking)
        {
            timeSinceAttack = 0;
            StartCoroutine(rippedAttackCoroutine());
        }
    }

    IEnumerator rippedAttackCoroutine()
{
    //anim.SetTrigger("rippedAttack");
    pState.attacking = true;

    yield return new WaitForSeconds(0.1f);

    float boostedDamage = damage * multiplierIfRipped;

    if (yAxis == 0 || yAxis < 0 && Grounded())
    {
        Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed, boostedDamage);
    }
    else if (yAxis > 0)
    {
        Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed, boostedDamage);
    }
    else if (yAxis < 0 && !Grounded())
    {
        Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed, boostedDamage);
    }

    yield return new WaitForSeconds(0.2f);

    pState.attacking = false;
}

IEnumerator AttackRoutine()
{
    anim.SetTrigger("Attack");
    pState.attacking = true;

    yield return new WaitForSeconds(0.1f);

    if (yAxis == 0 || yAxis < 0 && Grounded())
    {
        Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed, damage);
    }
    else if (yAxis > 0)
    {
        Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed, damage);
    }
    else if (yAxis < 0 && !Grounded())
    {
        Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed, damage);
    }

    yield return new WaitForSeconds(0.2f);

    pState.attacking = false;
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
        if (pState.locked) return;

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

        anim.SetBool("Jumping", !Grounded());
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
            jumpBufferCounter = jumpBufferCounter - Time.deltaTime * 10;
        }
    }
}