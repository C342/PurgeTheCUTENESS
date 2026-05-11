using UnityEngine;

public class FatOne : BaseEnemyClass
{
    [SerializeField] private float followDistance = 15f;
    [SerializeField] private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rb.gravityScale = 12f;
    }

    protected void Update()
    {
        base.Update();
        bool isMoving = false;

        if (!isRecoiling)
        {
            float distanceToPlayer = Vector2.Distance
            (
                transform.position,
                PlayerController.Instance.transform.position
            );

            if (distanceToPlayer <= followDistance)
            {
                Vector2 targetPos = new Vector2(
                    PlayerController.Instance.transform.position.x,
                    transform.position.y
                );

                Vector2 newPos = Vector2.MoveTowards
                (
                    transform.position,
                    targetPos,
                    speed * Time.deltaTime
                );

                isMoving = newPos != (Vector2)transform.position;

                transform.position = newPos;
            }
        }

        anim.SetBool("Walking", isMoving);
        FlipTowardsPlayer();
    }

    private void FlipTowardsPlayer()
    {
        if (PlayerController.Instance.transform.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }

    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
    }
}