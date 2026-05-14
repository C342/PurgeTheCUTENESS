using UnityEngine;
using System.Collections;

public class Crawler : BaseEnemyClass
{
    [SerializeField] private float followDistance = 20f;

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 2f;
    private bool isAttacking;
    private float attackTimer;

    protected void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rb.gravityScale = 12f;
    }

    protected void Update()
    {
        base.Update();

        attackTimer -= Time.deltaTime;

        bool isMoving = false;

        float distanceToPlayer = Vector2.Distance(
            transform.position,
            PlayerController.Instance.transform.position
        );

        if (!isRecoiling && !isAttacking)
        {
            if (distanceToPlayer <= followDistance)
            {
                if (distanceToPlayer > attackRange)
                {
                    Vector2 targetPos = new Vector2(
                        PlayerController.Instance.transform.position.x,
                        transform.position.y
                    );

                    Vector2 newPos = Vector2.MoveTowards(
                        transform.position,
                        targetPos,
                        speed * Time.deltaTime
                    );

                    isMoving = newPos != (Vector2)transform.position;

                        transform.position = newPos;
                    }
                    else
                    {
                        if (attackTimer <= 0)
                        {
                            isAttacking = true;
                            StartCoroutine(AttackRoutine());
                        }
                    }
                }
            }

        FlipTowardsPlayer();

        anim.SetBool("Walking", isMoving && !isAttacking);
    }

    IEnumerator AttackRoutine()
    {
        attackTimer = attackCooldown;

        anim.SetBool("Walking", false);

        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange + 0.5f)
        {
            PlayerController.Instance.TakeDamage(damage);
        }

        yield return new WaitForSeconds(2.5f);

        isAttacking = false;
    }

    private void FlipTowardsPlayer()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3
                (-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3
                (Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}