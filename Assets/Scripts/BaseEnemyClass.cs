using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemyClass : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    float recoilTimer;

    [SerializeField] protected PlayerController player;
    [SerializeField] protected float speed;

    [SerializeField] protected float damage;

    protected Rigidbody2D rb;
    protected Animator anim;

    protected private virtual void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    } 

    public void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;

        isRecoiling = true;

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(-_hitDirection * _hitForce * recoilFactor, ForceMode2D.Impulse);

        StartCoroutine(RecoilCooldown());
    }

    IEnumerator RecoilCooldown()
    {
        yield return new WaitForSeconds(recoilLength);

        isRecoiling = false;
    }

    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {
            Attack();
        }
    }
    protected virtual void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = rb.GetComponent<Animator>();
    }
}