using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float speed = 25f;
    [SerializeField] float lifetime = 1.5f;
    [SerializeField] float recoilForce = 8f;

    private Vector2 direction;
    private float damage;

    Vector2 moveDirection;

    public void Initialize(Vector2 direction, float damage)
    {
        direction = direction.normalized;

        moveDirection = direction.normalized;
        this.damage = damage;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemyClass enemy = collision.GetComponent<BaseEnemyClass>();

        if (enemy != null)
        {
            enemy.EnemyHit(damage, direction, recoilForce);

            Destroy(gameObject);
        }
    }
}