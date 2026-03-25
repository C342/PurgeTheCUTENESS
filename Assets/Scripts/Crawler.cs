using UnityEngine;

public class Crawler : BaseEnemyClass
{

    protected void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        rb.gravityScale = 12f;
    }

    protected void Update()
    {
        base.Update();
        if (!isRecoiling)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }
    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
    }
}