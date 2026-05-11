using UnityEngine;

public class Crawler : BaseEnemyClass
{
    [SerializeField] private float followDistance = 20f;
    private Animator anim;

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
        bool isMoving = false;
        bool isAttacking = false;

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

        float playerX = PlayerController.Instance.transform.position.x;
        Vector3 scale = transform.localScale;

        if (playerX > transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (playerX < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
        anim.SetBool("Walking", isMoving);
    }

    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);

        bool isAttacking = true;
    }
}