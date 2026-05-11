using System;
using System.Collections;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    [Header("Arm Attack Settings")]
    [SerializeField] float armAttackMultiplier = 2f;
    [SerializeField] bool armRipped = false;

    [Header("Recoil Settings")]
    [SerializeField] int recoilXSteps = 5;
    [SerializeField] int recoilYSteps = 5;
    [SerializeField] float recoilYSpeed = 100;
    [SerializeField] float recoilXSpeed = 100;
    int stepsXRecoiled, stepsYRecoiled;

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
        if (pState.invincible) return;

            health = Mathf.Clamp(health - Mathf.RoundToInt(_damage), 0, maxHealth);

            if (health <= 0)
            {
                Die();
                return;
            }

            StartCoroutine(StopTakingDamage());
    }

    IEnumerator StopTakingDamage()
    {
        pState.invincible = true;

        yield return new WaitForSeconds(1f);

        pState.invincible = false;
    }

    void Die()
    {
        rb.linearVelocity = Vector2.zero;
        
        pState.locked = true;

        this.enabled = false;

        //anim.SetTrigger("Death");

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        Destroy(gameObject, 0f);

        FadeManager.Instance.FadeAndRestart();

        return null;
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(health - Mathf.RoundToInt(damage), 0, maxHealth);
            }
        }
    }

        private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(
            _attackTransform.position,
            _attackArea,
            0,
            attackableLayer
        );

        HashSet<BaseEnemyClass> hitEnemies = new HashSet<BaseEnemyClass>(); // HERE

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            BaseEnemyClass enemy = objectsToHit[i].GetComponent<BaseEnemyClass>();

            if (enemy != null && !hitEnemies.Contains(enemy))
            {
                hitEnemies.Add(enemy);

                float finalDamage = armRipped ? damage * armAttackMultiplier : damage;

                enemy.EnemyHit(
                    finalDamage,
                    (transform.position - objectsToHit[i].transform.position).normalized,
                    _recoilStrength
                );

                _recoilDir = true;
            }
        }
    }

    void Update()
    {
        GetInputs();
        UpdateJumpVariable();

        if (pState.dashing) return;

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
        Recoil();
    }

    void GetInputs()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            armAttack();
        }

        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");
    }

    void armAttack()
    {
        if(armRipped) return;

        armRipped = true;

        StartCoroutine(armAttackLock());
        anim.SetTrigger("ripArm");
    }

    IEnumerator armAttackLock()
    {
        pState.locked = true;

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);

        pState.locked = false;
    }

    private void Move()
    {
        if (pState.locked) return;

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
        if(pState.locked) return;

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
        {
            timeSinceAttack += Time.deltaTime;

            if (attack && timeSinceAttack >= timeBetweenAttack && !pState.attacking)
            {
                timeSinceAttack = 0;
                StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        anim.SetTrigger("Attack");
        pState.attacking = true;

        yield return new WaitForSeconds(0.1f);

        if (yAxis == 0 || yAxis < 0 && Grounded())
        {
            Hit(SideAttackTransform, SideAttackArea, ref pState.recoilingX, recoilXSpeed);
        }
        else if (yAxis > 0)
        {
            Hit(UpAttackTransform, UpAttackArea, ref pState.recoilingY, recoilYSpeed);
        }
        else if (yAxis < 0 && !Grounded())
        {
            Hit(DownAttackTransform, DownAttackArea, ref pState.recoilingX, recoilXSpeed);
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
        if(pState.locked) return;

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